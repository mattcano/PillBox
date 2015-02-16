﻿using PillBox.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;
using Twilio.Mvc;
using Twilio.TwiML;
using Twilio.TwiML.Mvc;

namespace PillBox.Website.Controllers
{
    public class TwilioController : Controller
    {
        private PillBoxDbContext db = new PillBoxDbContext();

        public TwiMLResult Welcome()
        {
            var response = new TwilioResponse();

            string name;

            //if (request.To.Contains("301"))
            //    name = "Damola";
            //else
                name = "You";
            
            response.BeginGather(new { action = "http://ec2-54-67-55-4.us-west-1.compute.amazonaws.com/twilio/ProcessResponse", numDigits = "1" })
                    .Say("Hey "+ name +" ! This is a pillbox reminder. Have you taken your medicines?"
                            + "Press 1 for yes. Press 2 for no")
                    .EndGather();

            return new TwiMLResult(response);
        }        

        public ActionResult ProcessResponse(VoiceRequest request)
        {
            var response = new TwilioResponse();

            if (request.Digits == "1")
            {
                response.Say("Thank you for responding. We have you down as taking your medicine!");
            }
            else
            {
                response.Say("Ok why didnt you take your medicine?");
            }

            return new TwiMLResult(response);
        }

        public ActionResult CallAndGetResponse()
        {
            string accountSid = "ACa68cb3055a5c573f76862831c0995c48";
            string authToken = "8917e0e37320d868756ca59864dd29b6";
            var client = new TwilioRestClient(accountSid, authToken);

            var call =client.InitiateOutboundCall("6506668667",
                "3014373223",
                "http://ec2-54-67-55-4.us-west-1.compute.amazonaws.com/twilio/welcome");

                //client.SendMessage(
                //"6506668667",
                //"3014373223",
                //"Testing out twilio from .NET"
                //);

            return new TwiMLResult(new TwilioResponse());
        }

    }
}