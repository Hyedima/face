using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;


namespace IDS
{
    public static class Global
    {
        public static string LuxandKey = "vzm3vx/iIfmU4NsxPHciqHwP/fdsnVT4vo3MpwZvuI0e3oqsOjq1Gp4CeTC4m963GGJdSFwgR40MB3jdXKvT+IB9uuaFhdTS6Y5kbi/LXu4MqGkNDVHRKcP47VaP/djTvJFOsfP9gxH4qneFm/C5m0jHEzdPTc5O8tPmsC7EOoE=";

        public static TFaceRecord[] Persons;

        public static XElement RootElem;
        public static string xmlFilePath = Application.StartupPath + "\\AppSettings.xml";
        public static string ImagesFilePath = Application.StartupPath + "\\Images\\";

        public static CameraClass DefaultCamera;
        public static CameraClass[] CameraList;

        public static bool AutomaticTaining = false;

        public static string Fullname;
        public static string Title;
        public static string Gender;
        public static string ListType = "Open";

    }
}
