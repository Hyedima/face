using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Luxand;

namespace IDS
{
    public partial class InputName : Form
    {
        public string userName;
        public FSDK.CImage facePhoto;
        public TFaceRecord RealPhoto;

        FxClass fx = new FxClass();

        public InputName()
        {
            InitializeComponent();
        }

        public InputName(FSDK.CImage Photo)
        {
            InitializeComponent();

            facePhoto = Photo;
            Image frameImage = Photo.ToCLRImage();
            picBox.Image = frameImage;
        }

        public InputName(TFaceRecord Photo)
        {
            InitializeComponent();

            RealPhoto = Photo;
            facePhoto = Photo.faceImage;
            Image frameImage = Photo.faceImage.ToCLRImage();
            picBox.Image = frameImage;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            userName = txtNames.Text;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {                    
            TFaceRecord fr = new TFaceRecord();
            fr = RealPhoto;
            fr.ImageFileName = "NA";           

            fr.Fullname = txtName.Text.Trim();
            fr.Title = comTitle.Text.Trim();
            fr.Gender = comGender.Text.Trim();
            fr.ListType = "Open";

            if (Global.AutomaticTaining)
            {
                Global.Fullname = fr.Fullname;
                Global.Title = fr.Title;
                Global.Gender = fr.Gender;
                Global.ListType = "Open";
            }

            //fr.FacePosition = new FSDK.TFacePosition();
            //fr.FacialFeatures = new FSDK.TPoint[2];
            //fr.Template = new byte[FSDK.TemplateSize];

            //fr.image = facePhoto;

            //fr.FacePosition = fr.image.DetectFace();
            if (0 == fr.FacePosition.w)
            {
                MessageBox.Show("No face found. Try to lower the Minimal Face Quality parameter in the Options dialog box.\r\n", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                fr.faceImage = fr.image.CopyRect((int)(fr.FacePosition.xc - Math.Round(fr.FacePosition.w * 0.5)), (int)(fr.FacePosition.yc - Math.Round(fr.FacePosition.w * 0.5)), (int)(fr.FacePosition.xc + Math.Round(fr.FacePosition.w * 0.5)), (int)(fr.FacePosition.yc + Math.Round(fr.FacePosition.w * 0.5)));
                fr.FacialFeatures = fr.image.DetectEyesInRegion(ref fr.FacePosition);
                fr.Template = fr.image.GetFaceTemplateInRegion(ref fr.FacePosition); // get template with higher precision

                
                if(fx.AddPerson(fr))
                {
                    fx.GetPersons();
                    MessageBox.Show(fr.Fullname + " Has Been Added Successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {

                }

            }



            this.Close();
        }


        public void AutoSave()
        {
            TFaceRecord fr = new TFaceRecord();
            fr = RealPhoto;
            fr.ImageFileName = "NA";

            fr.Fullname = Global.Fullname;
            fr.Title = Global.Title;
            fr.Gender = Global.Gender;
            fr.ListType = "Open";

            userName = fr.Fullname;
            //fr.FacePosition = new FSDK.TFacePosition();
            //fr.FacialFeatures = new FSDK.TPoint[2];
            //fr.Template = new byte[FSDK.TemplateSize];

            //fr.image = facePhoto;

            //fr.FacePosition = fr.image.DetectFace();
            if (0 == fr.FacePosition.w)
            {

                //MessageBox.Show("No face found. Try to lower the Minimal Face Quality parameter in the Options dialog box.\r\n", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                fr.faceImage = fr.image.CopyRect((int)(fr.FacePosition.xc - Math.Round(fr.FacePosition.w * 0.5)), (int)(fr.FacePosition.yc - Math.Round(fr.FacePosition.w * 0.5)), (int)(fr.FacePosition.xc + Math.Round(fr.FacePosition.w * 0.5)), (int)(fr.FacePosition.yc + Math.Round(fr.FacePosition.w * 0.5)));
                fr.FacialFeatures = fr.image.DetectEyesInRegion(ref fr.FacePosition);
                fr.Template = fr.image.GetFaceTemplateInRegion(ref fr.FacePosition); // get template with higher precision


                if (fx.AddPerson(fr))
                {
                    fx.GetPersons();
                    //MessageBox.Show(fr.Fullname + " Has Been Added Successfully.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {

                }

            }
            
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkTraining_CheckedChanged(object sender, EventArgs e)
        {
            Global.AutomaticTaining = chkTraining.Checked;
        }
    }
}
