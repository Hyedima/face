using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.Drawing.Imaging;

using System.Runtime.InteropServices;

using System.Linq;
using System.Xml.Linq;
using System.Threading.Tasks;


using Luxand;
using System.Collections.Generic;

namespace IDS
{
    public struct TFaceRecord
    {
        public string Fullname;
        public string Title;
        public string Gender;
        public string ListType;

        public byte[] Template; //Face Template;
        public FSDK.TFacePosition FacePosition;
        public FSDK.TPoint[] FacialFeatures; //Facial Features;

        public string ImageFileName;

        public FSDK.CImage image;
        public FSDK.CImage faceImage;
    }


    class FxClass
    {

        string cnString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "/IDSData.mdb;";

        public FxClass()
        { 
            if (File.Exists(Global.xmlFilePath))
            {
                Global.RootElem = XElement.Load(Global.xmlFilePath);
            }
            else
            {
                try
                {
                    string xmlFirstLine = "<?xml version='1.0' encoding='utf-8'?><AppSettings></AppSettings>";
                    File.WriteAllText(Global.xmlFilePath, xmlFirstLine);
                    Global.RootElem = XElement.Load(Global.xmlFilePath);

                }
                catch (Exception n)
                {

                }

            }
        }


        /// Open file in to a filestream and read data in a byte array.
        /// </summary>
        /// <param name="sPath">Image file path.</param>
        /// <returns></returns>
        byte[] ReadFile(string sPath)
        {
            //Initialize byte array with a null value initially.
            byte[] data = null;

            //Use FileInfo object to get file size.
            FileInfo fInfo = new FileInfo(sPath);
            long numBytes = fInfo.Length;

            //Open FileStream to read file
            FileStream fStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);

            //Use BinaryReader to read file stream into byte array.
            BinaryReader br = new BinaryReader(fStream);

            //When you use BinaryReader, you need to supply number of bytes 
            //to read from file.
            //In this case we want to read entire file. 
            //So supplying total number of bytes.
            data = br.ReadBytes((int)numBytes);

            return data;
        }

        public string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
              byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            Image image = Image.FromStream(ms, true);
            return image;
        }

        public bool AddPerson(TFaceRecord fr)
        {
            bool ok = true;


            try
            {
                //preparing FaceRecord to save
                Image img = null;
                Image img_face = null;

                MemoryStream strm = new MemoryStream();
                MemoryStream strm_face = new MemoryStream();
                img = fr.image.ToCLRImage();
                img_face = fr.faceImage.ToCLRImage();
                img.Save(strm, System.Drawing.Imaging.ImageFormat.Jpeg);
                img_face.Save(strm_face, System.Drawing.Imaging.ImageFormat.Jpeg);

                byte[] img_array = new byte[strm.Length];
                byte[] img_face_array = new byte[strm_face.Length];

                strm.Position = 0;
                strm.Read(img_array, 0, img_array.Length);
                strm_face.Position = 0;
                strm_face.Read(img_face_array, 0, img_face_array.Length);


                OleDbConnection cn = new OleDbConnection();

                try
                {
                    cn.ConnectionString = cnString;
                    cn.Open();

                    //string sqlStr = "INSERT INTO PersonTable(Fname, Title, Gender, ListType, ImageFileName, FacePositionXc, FacePositionYc, FacePositionW, FacePositionAngle, Eye1X, Eye1Y, Eye2X, Eye2Y, Template, Image, FaceImage) Values(@Fname, @Title, @Gender, @ListType, @ImageFileName, @FacePositionXc, @FacePositionYc, @FacePositionW, @FacePositionAngle, @Eye1X, @Eye1Y, @Eye2X, @Eye2Y, @Template, @Image, @FaceImage)";
                    string sqlStr = "INSERT INTO PersonTable(Fname, Title, Gender, ListType, ImageFileName, FacePositionXc, FacePositionYc, FacePositionW, FacePositionAngle, Eye1X, Eye1Y, Eye2X, Eye2Y, Template, Imaged, FaceImage) Values(@Fname, @Title, @Gender, @ListType, @ImageFileName, @FacePositionXc, @FacePositionYc, @FacePositionW, @FacePositionAngle, @Eye1X, @Eye1Y, @Eye2X, @Eye2Y, @Template, @Image, @FaceImage)";
                    OleDbCommand oCmd = new OleDbCommand(sqlStr, cn);

                    oCmd.Parameters.Add("@Fname", OleDbType.VarChar);
                    oCmd.Parameters.Add("@Title", OleDbType.VarChar);
                    oCmd.Parameters.Add("@Gender", OleDbType.VarChar);
                    oCmd.Parameters.Add("@ListType", OleDbType.VarChar);

                    oCmd.Parameters.Add("@ImageFileName", OleDbType.VarChar);
                    oCmd.Parameters.Add("@FacePositionXc", OleDbType.Integer);
                    oCmd.Parameters.Add("@FacePositionYc", OleDbType.Integer);
                    oCmd.Parameters.Add("@FacePositionW", OleDbType.Integer);
                    oCmd.Parameters.Add("@FacePositionAngle", OleDbType.Single);

                    oCmd.Parameters.Add("@Eye1X", OleDbType.Integer);
                    oCmd.Parameters.Add("@Eye1Y", OleDbType.Integer);
                    oCmd.Parameters.Add("@Eye2X", OleDbType.Integer);
                    oCmd.Parameters.Add("@Eye2Y", OleDbType.Integer);

                    oCmd.Parameters.Add("@Template", OleDbType.VarChar);
                    oCmd.Parameters.Add("@Image", OleDbType.VarChar);
                    oCmd.Parameters.Add("@FaceImage", OleDbType.VarChar);


                    /*
                                                            oCmd.Parameters.Add("@Template", OleDbType.VarBinary);
                                        oCmd.Parameters.Add("@Image", OleDbType.VarBinary);
                                        oCmd.Parameters.Add("@FaceImage", OleDbType.VarBinary);
                    
                    */
                    oCmd.Parameters["@Fname"].Value = fr.Fullname;
                    oCmd.Parameters["@Title"].Value = fr.Title;
                    oCmd.Parameters["@Gender"].Value = fr.Gender;
                    oCmd.Parameters["@ListType"].Value = fr.ListType;

                    oCmd.Parameters["@ImageFileName"].Value = fr.ImageFileName;
                    oCmd.Parameters["@FacePositionXc"].Value = fr.FacePosition.xc;
                    oCmd.Parameters["@FacePositionYc"].Value = fr.FacePosition.yc;
                    oCmd.Parameters["@FacePositionW"].Value = fr.FacePosition.w;
                    oCmd.Parameters["@FacePositionAngle"].Value = (float)fr.FacePosition.angle;
                    
                    oCmd.Parameters["@Eye1X"].Value = fr.FacialFeatures[0].x;
                    oCmd.Parameters["@Eye1Y"].Value = fr.FacialFeatures[0].y;
                    oCmd.Parameters["@Eye2X"].Value = fr.FacialFeatures[1].x;
                    oCmd.Parameters["@Eye2Y"].Value = fr.FacialFeatures[1].y;


                    string template = Convert.ToBase64String(fr.Template);
                    oCmd.Parameters["@Template"].Value = template;

                    string image = Convert.ToBase64String(img_array);
                    oCmd.Parameters["@Image"].Value = image;

                    string faceImage = Convert.ToBase64String(img_face_array);
                    oCmd.Parameters["@FaceImage"].Value = faceImage;

                    int iresult = oCmd.ExecuteNonQuery();

                    img.Dispose();
                    img_face.Dispose();
                }
                catch (Exception ex)
                {
                    ok = false;
                    MessageBox.Show(ex.Message, "Exception on saving to database");
                }
                finally
                {
                    cn.Close();
                }

            }catch(Exception n)
            {
                ok = false;
                MessageBox.Show(n.Message);
            }


            return ok;
        }

        public TFaceRecord[] GetPersons()
        {
            TFaceRecord[] faces = null;


            using (OleDbConnection cn = new OleDbConnection())
            {
                try
                {
                    cn.ConnectionString = cnString;
                    cn.Open();

                    string sql = "SELECT * FROM PersonTable";
                    OleDbCommand cmd = new OleDbCommand(sql, cn);
                    OleDbDataAdapter da = new OleDbDataAdapter(cmd);

                    DataTable table = new DataTable();
                    da.Fill(table);


                    if (table.Rows.Count > 0)
                    {
                        faces = new TFaceRecord[table.Rows.Count];
                        int i = -1;

                        foreach(DataRow row in table.Rows)
                        {

                            TFaceRecord f = new TFaceRecord();

                            f.Fullname = row.ItemArray[1].ToString();
                            f.Title = row.ItemArray[2].ToString();
                            f.Gender = row.ItemArray[3].ToString();
                            f.ListType = row.ItemArray[4].ToString();

                            f.ImageFileName = row.ItemArray[5].ToString();

                            f.FacePosition = new FSDK.TFacePosition();
                            f.FacePosition.xc = Convert.ToInt32(row.ItemArray[6].ToString());
                            f.FacePosition.yc = Convert.ToInt32(row.ItemArray[7].ToString());
                            f.FacePosition.w = Convert.ToInt32(row.ItemArray[8].ToString());
                            f.FacePosition.angle = Convert.ToDouble(row.ItemArray[9].ToString());

                            f.FacialFeatures = new FSDK.TPoint[2];

                            f.FacialFeatures[0] = new FSDK.TPoint();
                            f.FacialFeatures[0].x = Convert.ToInt32(row.ItemArray[10].ToString());
                            f.FacialFeatures[0].y = Convert.ToInt32(row.ItemArray[11].ToString());

                            f.FacialFeatures[1] = new FSDK.TPoint();
                            f.FacialFeatures[1].x = Convert.ToInt32(row.ItemArray[12].ToString());
                            f.FacialFeatures[1].y = Convert.ToInt32(row.ItemArray[13].ToString());

                            f.Template = Convert.FromBase64String(row.ItemArray[14].ToString());

                            Image img = Base64ToImage(row.ItemArray[15].ToString());
                            f.image = new FSDK.CImage(img);

                            Image fImg = Base64ToImage(row.ItemArray[16].ToString());
                            f.faceImage = new FSDK.CImage(fImg);

                            i++;
                            faces[i] = f;
                        }

                        Global.Persons = faces;
                    }
                }
                catch (Exception n)
                {

                }
            }

            return faces;
        }

        public bool GetPerson(TFaceRecord faceIn, out TFaceRecord faceOut)
        {
            bool ok = false;
            
            TFaceRecord f = new TFaceRecord();

            if (Global.Persons == null) { faceOut = f; return ok; }

            try
            {
                float Threshold = 0.001155f;
                //FSDK.GetMatchingThresholdAtFAR(0.0009f, ref Threshold);

                List<float> sFaces = new List<float>();

                SimilarFaces[] Faces = null;
                SimilarFaces[] faces = new SimilarFaces[Global.Persons.Length];
                int i = -1;

                foreach ( TFaceRecord fac in Global.Persons.AsParallel())
                {
                    TFaceRecord F = new TFaceRecord();
                    F = fac;

                    float Similarity = 0.0f; //0.000026273356E == 0.00228634826
                    FSDK.MatchFaces(ref faceIn.Template, ref F.Template, ref Similarity);
                    if (Similarity >= Threshold)
                    {
                        i++;
                        faces[i].FullName = fac.Fullname;
                        faces[i].Similarity = Similarity;

                        ok = true;
                        f = fac;
                        break;
                    }

                }

                //Faces.OrderByDescending<SimilarFaces,>

                //Parallel.ForEach(Global.Persons.AsParallel(), fac =>
                // {

                //     float Similarity = 0.0f;
                //     FSDK.MatchFaces(ref faceIn.Template, ref fac.Template, ref Similarity);
                //     if (Similarity >= Threshold)
                //     {
                //         ok = true;
                //         f = fac;
                //     }

                // });

            }
            catch (Exception n)
            {

            }
            
            faceOut = f;
            return ok;
        }

        public void GetPerson(TFaceRecord faceIn, out string FaceFullName)
        {
            bool ok = false;
            string fName = "Unknown Face";

            if (Global.Persons == null) { FaceFullName = fName; return; }

            try
            {
                float Threshold = 0.034455f;
                //float Threshold = 0.001155f;

                SimilarFaces[] Faces = null;
                SimilarFaces[] faces = new SimilarFaces[Global.Persons.Length];
                List<SimilarFaces> Fa = new List<SimilarFaces>();
                int i = -1;
                
                foreach (TFaceRecord fac in Global.Persons)
                {
                    TFaceRecord F = new TFaceRecord();
                    F = fac;

                    float Similarity = 0.0f; 
                    FSDK.MatchFaces(ref faceIn.Template, ref F.Template, ref Similarity);
                    if (Similarity >= Threshold)
                    {
                        i++;
                        faces[i].FullName = fac.Fullname;
                        faces[i].Similarity = Similarity;
                    }
                }


                if (faces != null)
                {
                    Faces = faces.Where(v => v.FullName != null).ToArray();

                    fName = GetMostAcurateFace(Faces);
                    //fName = SortSimilarFaces(Faces)[0].FullName;
                }


            }
            catch (Exception n)
            {

            }

            FaceFullName = fName;
            //return ok;
        }

        public static float[] SortArray(float[] array)
        {
            int length = array.Length;
            float temp = array[0];

            for (int i = 0; i < length; i++)
            {
                for (int j = i + 1; j < length; j++)
                {
                    if (array[i] > array[j])
                    {
                        temp = array[i];
                        array[i] = array[j];
                        array[j] = temp;
                    }
                }
            }

            return array;
        }


        public static SimilarFaces[] SortSimilarFaces(SimilarFaces[] array)
        {
            int length = array.Length;
            float temp = array[0].Similarity;

            for (int i = 0; i < length; i++)
            {
                for (int j = i + 1; j < length; j++)
                {
                    if (array[i].Similarity > array[j].Similarity)
                    {
                        temp = array[i].Similarity;
                        array[i].Similarity = array[j].Similarity;
                        array[j].Similarity = temp;
                    }
                }
            }

            return array;
        }


        public static string GetMostAcurateFace(SimilarFaces[] array)
        {
            int length = array.Length;
            float faceASimilarity = array[0].Similarity;

            for (int i = 1; i < length; i++)
            {
                float faceBSimilarity = array[i].Similarity;
                if (faceASimilarity < faceBSimilarity)
                {
                    faceASimilarity = faceBSimilarity;
                }
            }
                SimilarFaces F = (from f in array
                                  where f.Similarity == faceASimilarity
                                  select f).SingleOrDefault();


                //for (int j = i + 1; j < length; j++)
                //{
                //    if (array[i].Similarity > array[j].Similarity)
                //    {
                //        temp = array[i].Similarity;
                //        array[i].Similarity = array[j].Similarity;
                //        array[j].Similarity = temp;
                //    }
                //}
            

            return F.FullName;
        }

        public bool SetDefaultCamera(CameraClass cam)
        {
            bool ok = true;

            try
            {
                XElement DKams = Global.RootElem.Element("DefaultCamera");
                if (DKams == null)
                {
                    Global.RootElem.Add(new XElement("DefaultCamera"));
                    DKams = Global.RootElem.Element("DefaultCamera");

                    XElement c = new XElement("Camera");
                    c.SetAttributeValue("Default", "true");
                    c.SetAttributeValue("Index", cam.CameraIndex);
                    c.SetValue(cam.CameraName);

                    DKams.Add(c);
                }
                else
                {
                    DKams = Global.RootElem.Element("DefaultCamera");

                    if (DKams.HasElements)
                    {
                        XElement c = DKams.Element("Camera");
                        c.SetAttributeValue("Default", "true");
                        c.SetAttributeValue("Index", cam.CameraIndex);
                        c.SetValue(cam.CameraName);
                    }
                    else
                    {
                        XElement c = new XElement("Camera");
                        c.SetAttributeValue("Default", "true");
                        c.SetAttributeValue("Index", cam.CameraIndex);
                        c.SetValue(cam.CameraName);

                        DKams.Add(c);
                    }
                }

                Global.RootElem.Save(Global.xmlFilePath, SaveOptions.None);
            }
            catch (Exception n)
            {
                ok = false;
            }


            return ok;
        }

        public CameraClass GetDefaultCamera()
        {
            CameraClass cam = new CameraClass();

            try
            {
                XElement DKams = Global.RootElem.Element("DefaultCamera");
                if (DKams == null)
                {

                    cam.CameraIndex = -1;
                    cam.CameraName = "NA";
                    cam.DefaultCamera = false;

                    return cam;
                }

                if (DKams.HasElements)
                {
                    XElement Kam = DKams.Element("Camera");

                    cam.CameraIndex = Convert.ToInt32(Kam.Attribute("Index").Value.Trim());
                    cam.CameraName = Kam.Value.Trim();
                    cam.DefaultCamera = Convert.ToBoolean(Kam.Attribute("Default").Value.Trim());
                }
            }
            catch (Exception n)
            {

            }


            return cam;
        }

        public bool SaveCameraList(CameraClass[] cams)
        {
            bool ok = true;

            try
            {
                XElement Kams = Global.RootElem.Element("Cameras");
                if (Kams == null)
                {
                    Global.RootElem.Add(new XElement("Cameras"));

                    Kams = Global.RootElem.Element("Cameras");
                    foreach (CameraClass cam in cams)
                    {
                        XElement c = new XElement("Camera");
                        c.SetAttributeValue("Default", "false");
                        c.SetAttributeValue("Index", cam.CameraIndex);
                        c.SetValue(cam.CameraName);
                        Kams.Add(c);
                    }

                }
                else
                {
                    Kams.Elements("Camera").Remove();

                    Kams = Global.RootElem.Element("Cameras");
                    foreach (CameraClass cam in cams)
                    {
                        XElement c = new XElement("Camera");

                        c.SetAttributeValue("Default", "false");
                        c.SetAttributeValue("Index", cam.CameraIndex);
                        c.SetValue(cam.CameraName);

                        Kams.Add(c);
                    }

                    //Global.RootElem.Add(new XElement("Cameras"));
                    //Kams = Global.RootElem.Element("Cameras");
                    //foreach (CameraClass cam in cams)
                    //{
                    //    Kams.Add(new XElement("Camera", cam.CameraName));
                    //}

                }


                Global.RootElem.Save(Global.xmlFilePath, SaveOptions.None);

            }
            catch (Exception n)
            {

            }


            return ok;
        }

    }
}
