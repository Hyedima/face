using System;
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
    public partial class EditPersonFrm : Form
    {
        bool PixChange = false;
        Person[] persons;
        FxClass Fx = new FxClass();

        public EditPersonFrm()
        {
            InitializeComponent();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim() == "") { return; }

            Person p = new Person();
            persons = p.SearchForPersons(txtSearch.Text.Trim(), dataGrid);

            if (dataGrid.RowCount > 0)
            {
                dataGrid.Show();
            }
            else
            {
                dataGrid.Hide();
            }

        }

        private void txtSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text.Trim().ToLower().Contains("search"))
            {
                txtSearch.Clear();
                txtSearch.Focus();
            }
        }

        private void dataGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string ID = persons[dataGrid.CurrentRow.Index].ID;

            comTitle.Text = persons[dataGrid.CurrentRow.Index].Title;
            txtFName.Text = persons[dataGrid.CurrentRow.Index].FullName;
            comSex.Text = persons[dataGrid.CurrentRow.Index].Gender;

            picImg.Image = Fx.Base64ToImage(persons[dataGrid.CurrentRow.Index].PassportImage);
            // MessageBox.Show(Num);

            comTitle.Enabled = true;
            txtFName.Enabled = true;
            comSex.Enabled = true;

            dataGrid.Hide();

            btnSave.Visible = true;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtFName.Text.Trim() == "")
            {
                MessageBox.Show("Person Name Cannot Be Empty", "Invalid Entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtFName.Focus();
                return;
            }

            string ID = persons[dataGrid.CurrentRow.Index].ID;

            Person p = new Person();

            p.Title = comTitle.Text.Trim();
            p.FullName = txtFName.Text.Trim();
            p.Gender = comSex.Text.Trim();

            if (PixChange)
            {
                p.PassportImage = Fx.ImageToBase64(picImg.Image, ImageFormat.Png);
            }

            if (p.UpdatePerson(ID, PixChange) == true)
            {
                MessageBox.Show("Person Updated Successfully.", "Intrusion Detection System", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error: Person Updating Failed.", "Intrusion Detection System", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void picImg_Click(object sender, EventArgs e)
        {
            PixChange = false;

            if (MessageBox.Show("Do You Want To Change This Pciture?", "Ïntrusion Detection", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) { return; }

            try
            {
                openFileDialog.InitialDirectory = @"C:\";
                openFileDialog.Title = "Select Passport Photo";
                openFileDialog.Filter = "Images Files (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF";

                openFileDialog.CheckFileExists = true;
                openFileDialog.CheckPathExists = true;


                openFileDialog.ShowDialog();

                picImg.Image = Image.FromFile(openFileDialog.FileName);
                PixChange = true;

            }
            catch (Exception q)
            {

            }

        }
    }
}
