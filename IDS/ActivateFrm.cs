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
using System.Threading;

namespace IDS
{
    public partial class ActivateFrm : Form
    {
        int delay = 5000;
        bool delayElapse = true;
        string IntruderFullName = "";
        bool MakingCall = false;
        bool call = false;
       // Timer timer = new Timer();

        FxClass Fx = new FxClass();

        // program states: whether we recognize faces, or user has clicked a face
        enum ProgramState { psRemember, psRecognize }
        //ProgramState programState = ProgramState.psRecognize;

        String cameraName;
        int cameraHandle = Global.DefaultCamera.CameraIndex;

        bool needClose = false;
        string userName;
        String TrackerMemoryFile = "tracker.dat";
        int mouseX = 0;
        int mouseY = 0;

        // WinAPI procedure to release HBITMAP handles returned by FSDKCam.GrabFrame
        [DllImport("gdi32.dll")]
        static extern bool DeleteObject(IntPtr hObject);

        public ActivateFrm()
        {
            InitializeComponent();           
        }

        private void ActivateFrm_Load(object sender, EventArgs e)
        {
            if (FSDK.FSDKE_OK != FSDK.ActivateLibrary(Global.LuxandKey))
            {
                MessageBox.Show("Invalid License Key Wizard", "Error activating IDS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            FSDK.InitializeLibrary();
            FSDKCam.InitializeCapturing();

            if (0 == Global.CameraList.Count())
            {
                MessageBox.Show("Please attach a camera", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            cameraName = Global.DefaultCamera.CameraName;// cameraList[cameraHandle];

            try
            {
                FSDKCam.VideoFormatInfo[] formatList;
                int count = 0;
                FSDKCam.GetVideoFormatList(ref cameraName, out formatList, out count);

                int VideoFormat = 0; // choose a video format
                imageBox.Width = formatList[VideoFormat].Width;
                imageBox.Height = formatList[VideoFormat].Height;

                //Width = formatList[VideoFormat].Width;
                //Height = formatList[VideoFormat].Height;

                Refresh();
            }
            catch(Exception n)
            {

            }

            

        }

        private void StartStreamingAndDetecting()
        {        
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

                int FrameCounter = 0;
                Pen P = new Pen(Color.AliceBlue);
                int L = 0;
                int T = 0;
                int W = 0;

                string[] Names = new string[10];

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
                        string FaceFullName = "Unknown Face";
//                        Name = "";
                        if (FaceFullName == "Unknown Face")
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


                            //TFaceRecord fOut = new TFaceRecord();
                            //string FaceFullName = "Unknown Face";

                            //if (FrameCounter < 9)
                            //{
                            //    FrameCounter++;

                            //    StringFormat format = new StringFormat();
                            //    format.Alignment = StringAlignment.Center;

                            //    gr.DrawString("Searching ...", new Font("Arial", 16), new SolidBrush(Color.LightGreen), facePosition.xc, top + w + 5, format);

                            //    Fx.GetPerson(fIn, out FaceFullName);
                            //    Names[FrameCounter] = FaceFullName;
                            //    name = FaceFullName;

                            //}
                            //else
                            //{

                            //FrameCounter++;
                            Fx.GetPerson(fIn, out FaceFullName);
                            Names[FrameCounter] = FaceFullName;
                            name = FaceFullName;
                            if (FaceFullName != "")
                            {
                                IntruderFullName = FaceFullName;
                            }
                            
                            FrameCounter = 0;


                            /*/ draw name
                            name = (from n in Names
                                    group n by n into g
                                    select new
                                    {
                                        Key = g.Key,
                                        Count = g.Count()
                                    }).OrderByDescending(g => g.Count).ToArray()[0].ToString();
                            */

                            StringFormat format = new StringFormat();
                            format.Alignment = StringAlignment.Center;

                            gr.DrawString(name, new Font("Arial", 16), new SolidBrush(Color.LightGreen), facePosition.xc, top + w + 5, format);

                            Pen pen = Pens.LightGreen;
                            gr.DrawRectangle(pen, left, top, w, w);


                            if (mouseX >= left && mouseX <= left + w && mouseY >= top && mouseY <= top + w)
                            {
                                pen = Pens.Blue;
                            }

                            P = pen;
                            L = left;
                            T = top;
                            W = w;

                            gr.DrawRectangle(pen, left, top, w, w);

                            if (MakingCall == false)
                            {
                                MakingCall = true;
                                // timer.Tick += timer_Tick;
                                //timer.Interval = 30000000;
                                // timer.Start();
                                MessageBox.Show("UNKNOWN");
                                Thread.Sleep(5000);
                                TwilioClass call = new TwilioClass();
                                if (call.MakeCall(IntruderFullName))
                                {
                                    MakingCall = false;
                                }
                            }
                            //}
                            //gr.DrawRectangle(P, L, T, W, W);
                            name = "";
                            IntruderFullName = "";
                        }
                    }

                    //programState = ProgramState.psRecognize;

                    // display current frame
                    imageBox.Image = frameImage;
                    GC.Collect(); // collect the garbage after the deletion
                }

                FSDKCam.CloseVideoCamera(cameraHandle);
                FSDKCam.FinalizeCapturing();

            }
            catch (Exception n)
            {
                StartStreamingAndDetecting();
            }
        }

        private void TrainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            needClose = true;
        }

        private void imageBox_MouseUp(object sender, MouseEventArgs e)
        {
            //programState = ProgramState.psRemember;
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

        private void timer_Tick(object sender, EventArgs e)
        {
            //timer.Stop();
            MakingCall = false;
            
        }

        private void StartTimer_Tick(object sender, EventArgs e)
        {
            StartTimer.Stop();
            StartStreamingAndDetecting();
        }

        private void ActivateFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            needClose = true;
        }

        private void imageBox_Resize(object sender, EventArgs e)
        {
            //PanelControl.Width = Width - imageBox.Width;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //            int w = PanelControl.Width;
            //          PanelControl.Width = w++;

//            int l = PanelControl.Left - 10;
 //           PanelControl.Left = l;

   //         Text = l.ToString();
        }
    }
}
