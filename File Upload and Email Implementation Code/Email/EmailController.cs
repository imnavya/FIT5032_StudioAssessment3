using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using OmniScanMRI.WebApp.Models;

namespace OmniScanMRI.WebApp.Controllers
{
    public class EmailController : Controller
    {
        // GET: SendEmail View
        public ActionResult SendEmail()
        {
            return View();
        }

        private OmniScanContext _context = new OmniScanContext();
        private readonly string _sendGridApiKey = "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";

        //POST: SendEmail transaction to send email
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Send(SendEmailModel model)
        {
            if (ModelState.IsValid)
            {
                // Getting the current logged-in user's Id
                //var userId = User.Identity.GetUserId();
                var userId = "user010";

                int doctorId = 2;  // set this to the desired value
                var doctor = _context.Doctors.FirstOrDefault(d => d.DoctorID == doctorId);
                var aspUser = doctor;

                var emailLog = new TrackEmail
                {
                    ToEmail = model.ToEmail,
                    Subject = model.Subject,
                    Content = model.Content,
                    SentByUserId = userId, 
                    SentDate = DateTime.Now,
                    CcEmail = model.CcEmail
                };

                _context.EmailLogs.Add(emailLog);
                _context.SaveChanges();

                var client = new SendGridClient(_sendGridApiKey);
                var from = new EmailAddress(aspUser.Email, "OmniScanMRI");
                var to = new EmailAddress(model.ToEmail);
                var msg = MailHelper.CreateSingleEmail(from, to, model.Subject, model.Content, model.Content);
                var response = await client.SendEmailAsync(msg);

                
                return RedirectToAction("EmailSuccess");

            }

            
            return View(model);
        }

        public ActionResult EmailSuccess()
        {
            return View();
        }
    }
}