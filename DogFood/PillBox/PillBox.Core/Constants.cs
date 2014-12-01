using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PillBox.Core
{
    public class Constants
    {
        //public const string DB_NAME = "Name=DefaultConnection";
        public static string DB_NAME
        {
            get
            {
                if (!DebuggingService.RunningInDebugMode())
                {


                    return "Name=DefaultConnection";
                    
                }
                else
                {
                    return "PillBoxDB";
                }
            }
        }
        public const string TEST_DB_NAME = "TEST_PillBoxDB";
        public const string EMAIL_ADDRESS = "pillbox.health@gmail.com";
        public const string PASSWORD = "k33py0url0v3d0n3sh3@lthy";

        // Emailer Information
        public const string GMAIL_SERVER = "smtp.gmail.com";
        public const int GMAIL_PORT = 587;

        // Twilio Info
        public const string TWILIO_ACCOUNTSID = "ACa68cb3055a5c573f76862831c0995c48";
        public const string TWILIO_AUTHTOKEN = "8917e0e37320d868756ca59864dd29b6";
        public const string TWILIO_NUMBER = "4248357603";
    }
}
