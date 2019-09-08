using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;


namespace IDS
{
    public partial class frm_test : Form
    {
        public frm_test()
        {
            InitializeComponent();
        }

        private void frm_test_Load(object sender, EventArgs e)
        {
            //+12562531418

            //15017122661
            // Find your Account Sid and Auth Token at twilio.com/console
            const string accountSid = "ACf2157b47bc63f8f6b9a3000c5d225b82";
            const string authToken = "470935f627365e3c7da83bc85e4373b3";
            TwilioClient.Init(accountSid, authToken);

            var to = new PhoneNumber("+2348137243528");
            var from = new PhoneNumber("+12562531418");
            var call = CallResource.Create(to, from,
                url: new Uri("https://demo.twilio.com/docs/voice.xml"));
            //label1.Text = call.Sid;
            Console.WriteLine(call.Sid);
        }
    }
}
