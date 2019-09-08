using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Twilio;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace IDS
{
    class CallClass
    {
        public CallClass()
        {

        }

        public void MakeCall(string personName)
        {
            // Your Account SID from twilio.com/console
//            var accountSid = "AC1f05db609ba8398a8b1c3664fb8f38d0";
            var accountSid = "AC1c716f11155003f530c56e4bacf44566";
            
            // Your Auth Token from twilio.com/console
//            var authToken = "d82287ecde01621d6a97454ed3ae12c0";
            var authToken = "b9a42e2a4902c4946e35062bcbfa0e91";

            try
            {
                TwilioClient.Init(accountSid, authToken);

                var call = CallResource.Create(
                    to: new PhoneNumber("+2348036420271"),
                    //to: new PhoneNumber("+2347037736311"),
                    from: new PhoneNumber("+12622870316"),
                    url: new Uri("https://018d5d5b.ngrok.io/ids.ng/voice.xml"));

                string callID = call.Sid.ToString();

            }
            catch (Exception n)
            {

            }


        }
    }
}
