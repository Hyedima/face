using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using Luxand;


namespace IDS
{
    public partial class TrainFrm : Form
    {
        FxClass Fx = new FxClass();

        // program states: whether we recognize faces, or user has clicked a face
        enum ProgramState { psRemember, psRecognize }
        ProgramState programState = ProgramState.psRecognize;

        String cameraName;
        int cameraHandle = Global.DefaultCamera.CameraIndex;

        bool needClose = false;
        string userName;
        string TrackerMemoryFile = "tracker.dat";
        int mouseX = 0;
        int mouseY = 0;

        // WinAPI procedure to release HBITMAP handles returned by FSDKCam.GrabFrame
        [DllImport("gdi32.dll")]
        static extern bool DeleteObject(IntPtr hObject);

        public TrainFrm()
        {
            InitializeComponent();

            Global.AutomaticTaining = false;
        }

        private void TrainFrm_Load(object sender, EventArgs e)
        {
            if (FSDK.FSDKE_OK != FSDK.ActivateLibrary(Global.LuxandKey))
            {
                MessageBox.Show("Invalid License Key Wizard", "Error activating IDS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            FSDK.InitializeLibrary();
            FSDKCam.InitializeCapturing();

            //string[] cameraList;
            //int count;
            //FSDKCam.GetCameraList(out cameraList, out count);

            if (0 == Global.CameraList.Count())
            {
                MessageBox.Show("Please attach a camera", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            cameraName = Global.DefaultCamera.CameraName;// cameraList[cameraHandle];

            FSDKCam.VideoFormatInfo[] formatList;
            int count = 0;
            FSDKCam.GetVideoFormatList(ref cameraName, out formatList, out count);

            int VideoFormat = 0; // choose a video format
            imageBox.Width = formatList[VideoFormat].Width;
            imageBox.Height = formatList[VideoFormat].Height;

            //this.Width = formatList[VideoFormat].Width + 48;
            //this.Height = formatList[VideoFormat].Height + 96;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            this.btnStart.Enabled = false;

            //int FSDK.SetHTTPProxy("192.168.43.1:8080", 8080, "", "");
            //int r = FSDKCam.OpenIPVideoCamera(FSDKCam.FSDK_VIDEOCOMPRESSIONTYPE.FSDK_MJPEG, "192.168.43.1", "", "", 60, ref cameraHandle);
            int r = FSDKCam.OpenVideoCamera(ref cameraName, ref cameraHandle);
            if (r != FSDK.FSDKE_OK)
            {
                MessageBox.Show("Error opening the first camera", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            try
            {
                int tracker = 0;    // creating a Tracker
                if (FSDK.FSDKE_OK != FSDK.LoadTrackerMemoryFromFile(ref tracker, TrackerMemoryFile)) // try to load saved tracker state
                    FSDK.CreateTracker(ref tracker); // if could not be loaded, create a new tracker

                int err = 0; // set realtime face detection parameters
                FSDK.SetTrackerMultipleParameters(tracker, "HandleArbitraryRotations=false; DetermineFaceRotationAngle=true; InternalResizeWidth=100; FaceDetectionThreshold=1;", ref err);
                FSDK.SetFaceDetectionParameters(false, true, 384);

                while (!needClose)
                {
                    Int32 imageHandle = 0;
                    if (FSDK.FSDKE_OK != FSDKCam.GrabFrame(cameraHandle, ref imageHandle)) // grab the current frame from the camera
                    {
                        Application.DoEvents();
                        continue;
                    }

                    FSDK.CImage image = new FSDK.CImage(imageHandle);

                    long[] IDs;
                    long faceCount = 0;
                    FSDK.FeedFrame(tracker, cameraHandle, image.ImageHandle, ref faceCount, out IDs, sizeof(long) * 256); // maximum of 256 faces detected
                    Array.Resize(ref IDs, (int)faceCount);


                    // make UI controls accessible (to find if the user clicked on a face)
                    Application.DoEvents();

                    Image frameImage = image.ToCLRImage();
                    Graphics gr = Graphics.FromImage(frameImage);

                    for (int i = 0; i < IDs.Length; ++i)
                    {
                        FSDK.TFacePosition facePosition = new FSDK.TFacePosition();
                        FSDK.GetTrackerFacePosition(tracker, cameraHandle, IDs[i], ref facePosition);

                        int left = facePosition.xc - (int)(facePosition.w * 0.6);
                        int top = facePosition.yc - (int)(facePosition.w * 0.5);
                        int w = (int)(facePosition.w * 1.2);

                        string name;

                        TFaceRecord fIn = new TFaceRecord();

                        fIn.FacePosition = new FSDK.TFacePosition();
                        fIn.FacialFeatures = new FSDK.TPoint[2];
                        fIn.Template = new byte[FSDK.TemplateSize];

                        FSDK.CImage img = new FSDK.CImage(image.ImageHandle);
                        img = image.CopyRect((int)(facePosition.xc - Math.Round(facePosition.w * 0.5)), (int)(facePosition.yc - Math.Round(facePosition.w * 0.5)), (int)(facePosition.xc + Math.Round(facePosition.w * 0.5)), (int)(facePosition.yc + Math.Round(facePosition.w * 0.5)));

                        fIn.image = img;
                        fIn.FacePosition = fIn.image.DetectFace();

                        fIn.faceImage = fIn.image.CopyRect((int)(fIn.FacePosition.xc - Math.Round(fIn.FacePosition.w * 0.5)), (int)(fIn.FacePosition.yc - Math.Round(fIn.FacePosition.w * 0.5)), (int)(fIn.FacePosition.xc + Math.Round(fIn.FacePosition.w * 0.5)), (int)(fIn.FacePosition.yc + Math.Round(fIn.FacePosition.w * 0.5)));
                        fIn.FacialFeatures = fIn.image.DetectEyesInRegion(ref fIn.FacePosition);
                        fIn.Template = fIn.image.GetFaceTemplateInRegion(ref fIn.FacePosition); // get template with higher precision


                        TFaceRecord fOut = new TFaceRecord();
                        //if (Fx.GetPerson(fIn, out fOut))
                        //{
                        //name = fOut.Fullname;

                        // draw name
                        //StringFormat format = new StringFormat();
                        //format.Alignment = StringAlignment.Center;

                        //lblPersons.Text = name;
                        // gr.DrawString(name, new Font("Arial", 16), new SolidBrush(Color.LightGreen), facePosition.xc, top + w + 5, format);

                        //}
                        //else                                                
                        //{
                        //lblPersons.Text = "";

                        if (0 != fIn.FacePosition.w)
                        {
                            //img = new FSDK.CImage(image.ImageHandle);
                            //img = fIn.image.CopyRect((int)(facePosition.xc - Math.Round(facePosition.w * 0.5)), (int)(facePosition.yc - Math.Round(facePosition.w * 0.5)), (int)(facePosition.xc + Math.Round(facePosition.w * 0.5)), (int)(facePosition.yc + Math.Round(facePosition.w * 0.5)));
                            if (Global.AutomaticTaining)
                            {
                                InputName inputName = new InputName(fIn);
                                inputName.AutoSave();
                                userName = inputName.userName;
                            }
                            else
                            {
                                InputName inputName = new InputName(fIn);
                                if (DialogResult.OK == inputName.ShowDialog(this))
                                {
                                    userName = inputName.userName;
                                    if (userName == null || userName.Length <= 0)
                                    {
                                        String s = "";
                                    }
                                    else
                                    {

                                    }
                                }
                            }
                            //}
                        }
                        
                        //int res = FSDK.GetAllNames(tracker, IDs[i], out name, 65536); // maximum of 65536 characters

                        /*/if (FSDK.FSDKE_OK == res && name.Length > 0)
                        if (name.Length > 0)
                        {
                            // draw name
                            //name = "Prince Daniel";
                            StringFormat format = new StringFormat();
                            format.Alignment = StringAlignment.Center;

                            gr.DrawString(name, new Font("Arial", 16), new SolidBrush(Color.LightGreen), facePosition.xc, top + w + 5, format);
                        }

                        */

                        Pen pen = Pens.LightGreen;
                        if (mouseX >= left && mouseX <= left + w && mouseY >= top && mouseY <= top + w)
                        {
                            pen = Pens.Blue;
                            //programState = ProgramState.psRemember;
                            if (ProgramState.psRemember == programState)
                            {
                                //FSDK.CImage img = new FSDK.CImage(image.ImageHandle);
                                img = new FSDK.CImage(image.ImageHandle);
                                img = image.CopyRect((int)(facePosition.xc - Math.Round(facePosition.w * 0.5)), (int)(facePosition.yc - Math.Round(facePosition.w * 0.5)), (int)(facePosition.xc + Math.Round(facePosition.w * 0.5)), (int)(facePosition.yc + Math.Round(facePosition.w * 0.5)));

                                if (Global.AutomaticTaining)
                                {
                                    InputName inputName = new InputName(img);
                                    inputName.AutoSave();
                                    userName = inputName.userName;
                                }
                                else
                                {
                                    InputName inputName = new InputName(img);
                                    if (DialogResult.OK == inputName.ShowDialog(this))
                                    {
                                        userName = inputName.userName;
                                        if (userName == null || userName.Length <= 0)
                                        {
                                            String s = "";
                                        }
                                        else
                                        {

                                        }
                                    }
                                }
                            }
                        }


                        gr.DrawRectangle(pen, left, top, w, w);
                    }

                    programState = ProgramState.psRecognize;

                    // display current frame
                    imageBox.Image = frameImage;
                    GC.Collect(); // collect the garbage after the deletion
                }

                FSDKCam.CloseVideoCamera(cameraHandle);
                FSDKCam.FinalizeCapturing();
            }
            catch (Exception n)
            {
                Application.DoEvents();
            }
        }

        private void TrainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            needClose = true;
        }

        private void imageBox_MouseUp(object sender, MouseEventArgs e)
        {
            programState = ProgramState.psRemember;
        }

        private void imageBox_MouseMove(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;

        }

        private void imageBox_MouseLeave(object sender, EventArgs e)
        {
            mouseX = 0;
            mouseY = 0;

        }
    }
}
