using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hps.Integrator.Net.Services;
using Hps.Integrator.Net.Entities;
using Hps.Integrator.Net.Infrastructure;
using Hps.Integrator.Net.Serialization;
using Hps.Integrator.Net.Abstractions;
using System.Net.Mail;
using System.Web.Helpers;

namespace end_to_end.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult sendEmail()
        {
            WebMail.SmtpServer = "my.smtpserver.com";
            WebMail.SmtpPort = 123;
            WebMail.EnableSsl = false;
            WebMail.UserName = "username";
            WebMail.Password = "password";
            WebMail.From = "email@mail.com";
            WebMail.SmtpUseDefaultCredentials = false;

            WebMail.Send(
                to: "anotheremail@mail.com",
                    subject: "SecureSubmit Payment",
                    body: "Congratulations, you have just completed a SecureSubmit payment!",
                    isBodyHtml: true
                );

            return View("Index");
        }

        [HttpPost]
        public ActionResult Index(int id = 0)
        {

            //sendEmail();
            
            string firstname = Request.Form["FirstName"];
            string lastname = Request.Form["LastName"];
            string phonenumber = Request.Form["PhoneNumber"];
            string email = Request.Form["Email"];
            string address = Request.Form["address"];
            string city = Request.Form["city"];
            string state = Request.Form["state"];
            string zip = Request.Form["zip"];
            string card_number = Request.Form["card_number"];
            string card_cvc = Request.Form["card_cvc"];
            string exp_month = Request.Form["exp_month"];
            string exp_year = Request.Form["exp_year"];


            var config = new HpsServicesConfig() { SecretAPIKey = "skapi_cert_MVl7AQB1DkgAun1Ce771jrR-Mq8ZC03wDtrxLUPM0w" };

            var chargeService = new HpsCreditService(config);

            var cardHolder = new HpsCardHolder()
            {
                Address = new HpsAddress()
                {
                    Address = address,
                    Zip = zip,
                    State = state
                }
            };

            var creditCard = new HpsCreditCard
            {
                Cvv = Convert.ToInt32(card_cvc),
                ExpMonth = Convert.ToInt32(exp_month),
                ExpYear = Convert.ToInt32(exp_year),
                Number = card_number
            };
            try
            {
                var result = chargeService.Charge(10, "usd", creditCard);
                var auth = result.AuthorizationCode;
            }
            catch (InvalidRequestException e)
            { }
            catch (AuthenticationException e)
            { }
            catch (CardException e)
            { }

            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
