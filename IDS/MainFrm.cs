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
    public partial class MainFrm : Form
    {
        bool bClosed = true;
        FxClass Fx = new FxClass();

        int CameraCount = 0;
        string[] CameraList = null;

        public MainFrm()
        {
            InitializeComponent();

            if (FSDK.FSDKE_OK != FSDK.ActivateLibrary(Global.LuxandKey))
            {
                MessageBox.Show("Invalid License Key Wizard", "Error activating IDS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            FSDK.InitializeLibrary();

            DisplayCameraOnMenu();

            Fx.GetPersons();

            //Size = new Size(100, 100);
        }

        private void MainFrm_Load(object sender, EventArgs e)
        {
            this.BringToFront();

            //this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - Width, Screen.PrimaryScreen.WorkingArea.Height - Height);
        }

        private CameraClass[] GetCameras()
        {
            CameraClass[] cam = null;

            int i = FSDKCam.GetCameraList(out CameraList, out CameraCount);

            cam = new CameraClass[CameraCount];

            for (int c = 0; c < CameraCount; c++)
            {
                CameraClass ca = new CameraClass();
                ca.CameraIndex = c;
                ca.CameraName = CameraList[c];

                cam[c] = ca;
            }

            return cam;
        }

        private void DisplayCameraOnMenu()
        {
            defaultCameraMenu.DropDownItems.Clear();

            CameraClass[] cams = GetCameras();
            Global.CameraList = cams;

            if (Fx.SaveCameraList(cams))
            {
                foreach (CameraClass cam in cams)
                {
                    ToolStripMenuItem MenuItem = new ToolStripMenuItem();

                    MenuItem.Name = cam.CameraName;
                    MenuItem.Text = cam.CameraName;
                    MenuItem.Tag = cam.CameraIndex + "~" + cam.DefaultCamera + "~" + cam.DefaultCamera;

                    if(cam.CameraName == Fx.GetDefaultCamera().CameraName)
                    {
                        Global.DefaultCamera = cam;

                        Font font = MenuItem.Font;
                        MenuItem.Font = new Font(font, FontStyle.Bold);
                        MenuItem.Checked = true;
                    }

                    MenuItem.Click += new EventHandler(MakeThisCameraDefault);

                    defaultCameraMenu.DropDownItems.Add(MenuItem);
                }
            }
        }

        private void MakeThisCameraDefault(object sender, EventArgs e)
        {

            foreach(ToolStripMenuItem item in defaultCameraMenu.DropDownItems)
            {
                Font font = item.Font;
                item.Font = new Font(font, FontStyle.Regular);
                item.Checked = false;
            }

            ToolStripMenuItem MenuItem = (ToolStripMenuItem)sender;
            CameraClass cam = new CameraClass();

            cam.CameraName = MenuItem.Name;
            
            string[] tag = MenuItem.Tag.ToString().Split('~');
            cam.CameraIndex = Convert.ToInt32(tag[0]);
            cam.DefaultCamera = true;

            if (Fx.SetDefaultCamera(cam))
            {
                Global.DefaultCamera = cam;
                MessageBox.Show(cam.CameraName + " Is Now The Default Camera.", "Setting Default Camera", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Font font = MenuItem.Font;
                MenuItem.Font = new Font(font, FontStyle.Bold);
                MenuItem.Checked = true;
            }
            else
            {
                MessageBox.Show("Setting " + cam.CameraName + " Has The Default Camera Failed.", "Seeting Default Camera", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Font font = MenuItem.Font;
                MenuItem.Font = new Font(font, FontStyle.Regular);
                MenuItem.Checked = false;

            }

        }

        private void MnuActivateIDS_Click(object sender, EventArgs e)
        {
            ActivateFrm Frm = new ActivateFrm();
            //            Frm.TopMost = true;
            //
            Frm.Icon = Icon;
            //Frm.ShowDialog(this);
            Frm.Show();

            return;
        }

        private void MainFrm_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                contextMenu.Show(Cursor.Position);
            }
        }

        private void MnuExit_Click(object sender, EventArgs e)
        {
            DialogResult ans = MessageBox.Show("Do You Want To Close IDS?", "Intrusion Detection System", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);

            if (ans == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void MnuMinimize_Click(object sender, EventArgs e)
        {
            CallClass call = new CallClass();
            call.MakeCall("Prince");   
            return;

            if (WindowState == FormWindowState.Minimized)
            {
                MnuMinimize.Text = "Minimize IDS";
                Show();
                WindowState = FormWindowState.Normal;
                notifyIcon.Visible = false;
            }
            else
            {
                this.WindowState = FormWindowState.Minimized;
                notifyIcon.Visible = true;
                this.Hide();
                MnuMinimize.Text = "Show IDS";
            }
        }
                
        private void MainFrm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(2000);
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        private void MainFrm_MouseEnter(object sender, EventArgs e)
        {
            //if(WindowState==FormWindowState.Minimized)
            //{
               // notifyIcon.ShowBalloonTip(2000);
            //}
        }

        private void MainFrm_MouseHover(object sender, EventArgs e)
        {
            notifyIcon.ShowBalloonTip(1000);
        }

        private void notifyIcon_MouseMove(object sender, MouseEventArgs e)
        {            
            if (!bClosed) { return; }
            notifyIcon.ShowBalloonTip(500);
            bClosed = false;
        }

        private void notifyIcon_BalloonTipClosed(object sender, EventArgs e)
        {
            bClosed = true;
        }

        private void MnuAddPerson_Click(object sender, EventArgs e)
        {
            PersonFrm Frm = new PersonFrm();
            Frm.ShowDialog(this);
        }

        private void MnuEditPerson_Click(object sender, EventArgs e)
        {
            EditPersonFrm Frm = new EditPersonFrm();
            Frm.ShowDialog(this);
        }

        private void MnuWhiteListAPerson_Click(object sender, EventArgs e)
        {
            ListPersonFrm Frm = new ListPersonFrm();
            Frm.ShowDialog(this);
        }

        private void MnuTrain_Click(object sender, EventArgs e)
        {
            //EyeFrm Frm = new EyeFrm();
            //Frm.ShowDialog(this);

            TrainFrm Frm = new TrainFrm();
            Frm.Show();
            //Frm.ShowDialog(this);

        }

        private void btnmenu_Click(object sender, EventArgs e)
        {
            contextMenu.Show(Cursor.Position);
        }

        private void btnedit_Click(object sender, EventArgs e)
        {
            EditPersonFrm Frm = new EditPersonFrm();
            Frm.ShowDialog(this);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            PersonFrm Frm = new PersonFrm();
            Frm.ShowDialog(this);
        }

        private void btntrain_Click(object sender, EventArgs e)
        {
            TrainFrm Frm = new TrainFrm();
            Frm.ShowDialog(this);
        }

        private void btnactivate_Click(object sender, EventArgs e)
        {
            ActivateFrm Frm = new ActivateFrm();
            //Frm.TopMost = true;
            //
            Frm.Icon = Icon;
            Frm.ShowDialog(this);
            //Frm.Show();
            return;
        }
    }
}