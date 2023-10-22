using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OmniScanMRI.WebApp.Controllers
{
    
    public class AppointmentController : Controller
    {
        public ActionResult BookAppointment()
        {
            if (!User.Identity.IsAuthenticated)
            {
                
                Session["RedirectToBooking"] = true;

                var testSession = Session["RedirectToBooking"];
                System.Diagnostics.Debug.WriteLine($"BookAppointment session value: {testSession}");
                return RedirectToAction("Login", "Account");
            }

            return View();
        }

    }

}