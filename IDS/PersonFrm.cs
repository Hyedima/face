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
using System.Drawing.Imaging;

using Luxand;

namespace IDS
{
    public partial class PersonFrm : Form
    {
        // WinAPI procedure to release HBITMAP handles returned by FSDKCam.GrabFrame
        [DllImport("gdi32.dll")]
        static extern bool DeleteObject(IntPtr hObject);

        FxClass Fx = new FxClass();
        byte[] faceTemplate;

        public PersonFrm()
        {
            InitializeComponent();            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void PersonFrm_Load(object sender, EventArgs e)
        {
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - (Width + 200), Screen.PrimaryScreen.WorkingArea.Height - (Height + 200));
            comTitle.SelectedIndex = 0;
            comSex.SelectedIndex = 0;


            if (FSDK.FSDKE_OK != FSDK.ActivateLibrary("vzm3vx/iIfmU4NsxPHciqHwP/fdsnVT4vo3MpwZvuI0e3oqsOjq1Gp4CeTC4m963GGJdSFwgR40MB3jdXKvT+IB9uuaFhdTS6Y5kbi/LXu4MqGkNDVHRKcP47VaP/djTvJFOsfP9gxH4qneFm/C5m0jHEzdPTc5O8tPmsC7EOoE="))
            {
                MessageBox.Show("Please run the License Key Wizard (Start - Luxand - FaceSDK - License Key Wizard)", "Error activating FaceSDK", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            FSDK.InitializeLibrary();
            FSDK.SetFaceDetectionParameters(true, true, 384);
        }

        private void picImg_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Do You Want To Change This Pciture?","Ïntrusion Detection", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) { return; }

            try
            {
                openFileDialog.InitialDirectory = @"C:\";
                openFileDialog.RestoreDirectory = false;
                openFileDialog.Title = "Select Passport Photo";
                openFileDialog.Filter = "Images Files (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF";

                openFileDialog.CheckFileExists = true;
                openFileDialog.CheckPathExists = true;


//                openFileDialog.ShowDialog();

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        FSDK.CImage image = new FSDK.CImage(openFileDialog.FileName);

                        // resize image to fit the window width
                        double ratio = System.Math.Min((picImg.Width + 0.4) / image.Width,
                            (picImg.Height + 0.4) / image.Height);
                        image = image.Resize(ratio);

                        Image frameImage = image.ToCLRImage();
                        Graphics gr = Graphics.FromImage(frameImage);

                        FSDK.TFacePosition facePosition = image.DetectFace();
                        if (0 == facePosition.w)
                            MessageBox.Show("No faces detected", "Face Detection");
                        else
                        {
                            int left = facePosition.xc - (int)(facePosition.w * 0.6f);
                            int top = facePosition.yc - (int)(facePosition.w * 0.5f);
                            gr.DrawRectangle(Pens.LightGreen, left, top, (int)(facePosition.w * 1.2), (int)(facePosition.w * 1.2));


                            faceTemplate = new byte[FSDK.TemplateSize];
                            FSDK.GetFaceTemplateInRegion(image.ImageHandle, ref facePosition, out faceTemplate);
                                //GetFaceTemplate(image, out templateData);


                            FSDK.TPoint[] facialFeatures = image.DetectFacialFeaturesInRegion(ref facePosition);
                            int i = 0;
                            foreach (FSDK.TPoint point in facialFeatures)
                                gr.DrawEllipse((++i > 2) ? Pens.LightGreen : Pens.Blue, point.x, point.y, 3, 3);

                            gr.Flush();
                        }

                        // display image
                        picImg.Image = frameImage;
                        picImg.Refresh();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Exception");
                    }
                }

                //picImg.Image = Image.FromFile(openFileDialog.FileName);

            }
            catch(Exception q)
            {

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtFName.Text.Trim() == "")
            {
                MessageBox.Show("Person Name Cannot Be Empty", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtFName.Focus();
                return;
            }

            Person p = new Person();

            p.Title = comTitle.Text.Trim();
            p.FullName = txtFName.Text.Trim();
            p.Gender = comSex.Text.Trim();
            p.PassportImage = Fx.ImageToBase64(picImg.Image, ImageFormat.Png);
            p.FaceTemplate = faceTemplate;

            if (p.SavePerson() == true)
            {
                MessageBox.Show("Person Added Successfully.", "Intrusion Detection System", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
    }
}
