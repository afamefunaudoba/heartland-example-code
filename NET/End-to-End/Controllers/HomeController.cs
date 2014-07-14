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

namespace End_to_End.Controllers
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
        public ActionResult Index(End_to_End.Models.UsersContext.UserModel model)
        {

            sendEmail();

            var config = new HpsServicesConfig() { SecretAPIKey = "SECRET_API_KEY_HERE" };

            var chargeService = new HpsCreditService(config);

            var cardHolder = new HpsCardHolder()
            {
                Address = new HpsAddress()
                {
                    Address = model.address,
                    Zip = model.zipcode,
                    State = model.state
                }
            };

            var creditCard = new HpsCreditCard
            {
                Cvv = model.cvv,
                ExpMonth = model.expMonth,
                ExpYear = model.expYear,
                Number = model.number
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

            return View(model);
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
