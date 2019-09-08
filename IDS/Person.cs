using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.Threading;


namespace IDS
{
    class Person
    {
        string cnString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + @"\IDSData.mdb;";
        string sqlString = "";

        public string ID { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string ListType { get; set; }
        public string PassportImage { get; set; }
        public byte[] FaceTemplate { get; set; }


        public bool SavePerson()
        {
            bool ok = true;

            OleDbConnection cn = new OleDbConnection();

            try
            {
                cn.ConnectionString = cnString;
                cn.Open();

                sqlString = "INSERT INTO PersonTable(Title,FName,Gender,Img,template) VALUES('" + Title + "','" + FullName + "','" + Gender + "','" + PassportImage + "','" + FaceTemplate.ToString() + "')";
                OleDbCommand cmd = new OleDbCommand(sqlString, cn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception n)
            {
                ok = false;
            }
            finally
            {
                cn.Close();
            }


            return ok;
        }

        public Person[] GetPersons()
        {
            int i = -1;
            Person[] pers = null;
            OleDbConnection cn = new OleDbConnection();

            try
            {
                cn.ConnectionString = cnString;
                cn.Open();

                sqlString = "SELECT * FROM PersonTable";
                DataTable table = new DataTable();
                OleDbDataAdapter Da = new OleDbDataAdapter(sqlString, cn);

                Da.Fill(table);

                pers = new Person[table.Rows.Count];

                foreach (DataRow row in table.Rows)
                {
                    Person p = new Person();

                    p.ID = row.ItemArray[0].ToString().Trim();
                    p.Title = row.ItemArray[2].ToString().Trim();
                    p.FullName = row.ItemArray[1].ToString().Trim();
                    p.Gender = row.ItemArray[3].ToString().Trim();
                    p.ListType = row.ItemArray[4].ToString().Trim();
                    p.PassportImage = row.ItemArray[5].ToString().Trim();
                    p.FaceTemplate = Encoding.UTF8.GetBytes(row.ItemArray[6].ToString());

                    i++;

                    pers[i] = p;
                }

/*
                Parallel.ForEach(table.Rows, (DataRow)row =>
                {
                    Person p = new Person();

                    p.ID = row.ItemArray[0].ToString().Trim();
                    p.Title = row.ItemArray[2].ToString().Trim();
                    p.FullName = row.ItemArray[1].ToString().Trim();
                    p.Gender = row.ItemArray[3].ToString().Trim();
                    p.ListType = row.ItemArray[4].ToString().Trim();
                    p.PassportImage = row.ItemArray[5].ToString().Trim();

                    i++;

                    pers[i] = p;
                });
                */
            }
            catch (Exception n)
            {
            }
            finally
            {
                cn.Close();
            }


            return pers;
        }

        public Person[] SearchForPersons(string letters, DataGridView grid)
        {
            int i = -1;
            Person[] pers = null;
            OleDbConnection cn = new OleDbConnection();

            grid.Rows.Clear();

            try
            {
                cn.ConnectionString = cnString;
                cn.Open();

                sqlString = "SELECT * FROM PersonTable WHERE FName LIKE '%" + letters.Trim() + "%'";
                DataTable table = new DataTable();
                OleDbDataAdapter Da = new OleDbDataAdapter(sqlString, cn);

                Da.Fill(table);

                pers = new Person[table.Rows.Count];

                foreach (DataRow row in table.Rows)
                {
                    Person p = new Person();

                    p.ID = row.ItemArray[0].ToString().Trim();
                    p.Title = row.ItemArray[2].ToString().Trim();
                    p.FullName = row.ItemArray[1].ToString().Trim();
                    p.Gender = row.ItemArray[3].ToString().Trim();
                    p.ListType = row.ItemArray[4].ToString().Trim();
                    p.PassportImage = row.ItemArray[5].ToString().Trim();
                    p.FaceTemplate = Encoding.UTF8.GetBytes(row.ItemArray[6].ToString());

                    i++;

                    pers[i] = p;
                    grid.Rows.Add(p.FullName);
                }
            }
            catch (Exception n)
            {
            }
            finally
            {
                cn.Close();
            }


            return pers;
        }

        public bool UpdatePerson(string PersonID, bool PictureChanged = false)
        {
            bool ok = true;

            OleDbConnection cn = new OleDbConnection();

            try
            {
                cn.ConnectionString = cnString;
                cn.Open();

                if (PictureChanged)
                {
                    //sqlString = "UPDATE PersonTable SET Title='" + Title + "',FName='" + FullName + "',Gender='" + Gender + "',Img='" + PassportImage + "' WHERE ID=" + PersonID + "";
                    sqlString = "UPDATE PersonTable SET Img=" + PassportImage.Trim() + " WHERE ID=" + PersonID + "";

                }
                else
                {
                    sqlString = "UPDATE PersonTable SET Title='" + Title + "',FName='" + FullName + "',Gender='" + Gender + "' WHERE ID=" + PersonID + "";
                }
                OleDbCommand cmd = new OleDbCommand(sqlString, cn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception n)
            {
                ok = false;
            }
            finally
            {
                cn.Close();
            }


            return ok;
        }

        public bool BlackListThisPerson(string PersonID)
        {
            bool ok = true;

            OleDbConnection cn = new OleDbConnection();

            try
            {
                cn.ConnectionString = cnString;
                cn.Open();

                sqlString = "UPDATE PersonTable SET ListType='Black' WHERE ID=" + PersonID + "";
                OleDbCommand cmd = new OleDbCommand(sqlString, cn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception n)
            {
                ok = false;
            }
            finally
            {
                cn.Close();
            }

            return ok;
        }

        public bool WhiteListThisPerson(string PersonID)
        {
            bool ok = true;

            OleDbConnection cn = new OleDbConnection();

            try
            {
                cn.ConnectionString = cnString;
                cn.Open();

                sqlString = "UPDATE PersonTable SET ListType='White' WHERE ID=" + PersonID + "";
                OleDbCommand cmd = new OleDbCommand(sqlString, cn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception n)
            {
                ok = false;
            }
            finally
            {
                cn.Close();
            }

            return ok;
        }

        public bool RemoveFromAnyList(string PersonID)
        {
            bool ok = true;

            OleDbConnection cn = new OleDbConnection();

            try
            {
                cn.ConnectionString = cnString;
                cn.Open();

                sqlString = "UPDATE PersonTable SET ListType='Open' WHERE ID=" + PersonID + "";
                OleDbCommand cmd = new OleDbCommand(sqlString, cn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception n)
            {
                ok = false;
            }
            finally
            {
                cn.Close();
            }

            return ok;
        }
    }
}
