using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.TwiML;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using System.Net;

namespace IDS
{
    class TwilioClass
    {
        public TwilioClass()
        {
        }

        public bool MakeCall(string personName)
        {
            bool ok = true;
            // Your Account SID from twilio.com/console
            // var AccountSid = "AC1f05db609ba8398a8b1c3664fb8f38d0";
            //var AccountSid = "AC1c716f11155003f530c56e4bacf44566";

            // Your Auth Token from twilio.com/console
            //var AuthToken = "d82287ecde01621d6a97454ed3ae12c0";
            //var AuthToken = "b9a42e2a4902c4946e35062bcbfa0e91";

            const string AccountSid = "ACf2157b47bc63f8f6b9a3000c5d225b82";
            const string AuthToken = "470935f627365e3c7da83bc85e4373b3";

            try
            {
                string filename = @"C:\Users\DM\Desktop\abrak\Easy\ids.ng\voice.xml";
                string xml = "<?xml version='1.0' encoding='UTF - 8'?>";
                xml = "\n" + "<Response><Say voice='alice'>Welcome to Intrusion Detection System.</Say><Pause length = '1' /><Say voice = 'alice'>" + personName+" is In confined perimeter. Thanks for calling!</Say></Response>";

                File.WriteAllText(filename, xml);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                               | SecurityProtocolType.Tls11
                                               | SecurityProtocolType.Tls12
                                               | SecurityProtocolType.Ssl3;
                TwilioClient.Init(AccountSid, AuthToken);

                var call = CallResource.Create(
                    method: "GET",
                    to: new PhoneNumber("+2348137243528"),
                    //to: new PhoneNumber("+2347037736311"),
                    from: new PhoneNumber("+1 256 253 1418"),
                    //from: new PhoneNumber("+12622870316"),
                    url: new Uri("http://1ec6d9eb.ngrok.io/ids.ng/voice.xml"));
                var Sid = call.Sid;
            }
            catch (Exception n)
            {
                ok = false;
            }
            return ok;
        }
    }
}
