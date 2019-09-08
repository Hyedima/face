using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IDS
{
    public partial class ListPersonFrm : Form
    {
        Person[] persons;
        FxClass Fx = new FxClass();

        public ListPersonFrm()
        {
            InitializeComponent();
            comListType.SelectedIndex = 0;
        }

        private void ListPersonFrm_Load(object sender, EventArgs e)
        {

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

        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            if(comListType.SelectedIndex==0)
            {
                MessageBox.Show("Select A List to set", "Intrusion Detection System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string ID = persons[dataGrid.CurrentRow.Index].ID;
            string FullName = persons[dataGrid.CurrentRow.Index].FullName;

            Person p = new Person();

            if(comListType.Text.Trim().ToLower().Contains("white"))
            {
                if (p.WhiteListThisPerson(ID))
                {
                    MessageBox.Show( FullName + " Has Been WhiteListed", "Intrusion Detection System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("White Listing " + FullName + " Has Failed", "Intrusion Detection System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (comListType.Text.Trim().ToLower().Contains("black"))
            {
                if (p.BlackListThisPerson(ID))
                {
                    MessageBox.Show(FullName + " Has Been BlackListed", "Intrusion Detection System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Black Listing " + FullName + " Has Failed", "Intrusion Detection System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (p.RemoveFromAnyList(ID))
                {
                    MessageBox.Show(FullName + " Has Been Removed From All List", "Intrusion Detection System", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Removing " + FullName + " From All List Has Failed", "Intrusion Detection System", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }



        }

        private void dataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string ID = persons[dataGrid.CurrentRow.Index].ID;

            txtFName.Text = persons[dataGrid.CurrentRow.Index].FullName;
            picImg.Image = Fx.Base64ToImage(persons[dataGrid.CurrentRow.Index].PassportImage);

            txtFName.Enabled = true;

            dataGrid.Hide();

            btnSave.Visible = true;

        }
    }
}
