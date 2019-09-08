using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;


namespace IDS
{
    public partial class EyeFrm : Form
    {
        FxClass Fx = new FxClass();

        List<string> labels = new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;
        string name, names = null;

        public EyeFrm()
        {
            InitializeComponent();

            //txtName.Hide();

            Person p = new Person();
            comPersons.DataSource = p.GetPersons();
            comPersons.DisplayMember = "FullName";
            comPersons.ValueMember = "ID";

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void PersonFrm_Load(object sender, EventArgs e)
        {
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - (Width + 200), Screen.PrimaryScreen.WorkingArea.Height - (Height + 200));
        }


        private void btnDetect_Click(object sender, EventArgs e)
        {
            btnDetect.Enabled = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show("Hi");
                //Trained face counter
                ContTrain = ContTrain + 1;

                //Get a gray frame from capture device

                //Face Detector                

//                MessageBox.Show(txtName.Text + "´s face detected and added :)", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception n)
            {
                //MessageBox.Show(n.Message);
                MessageBox.Show("Enable the face detection first", "Training Fail", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

    }
}
