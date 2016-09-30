using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccessControlManagement.Models;

namespace AccessControlManagement.Controllers
{
    public class RegistrationController : Controller
    {
        // GET: Registration
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(user u)
        {
            if (ModelState.IsValid)
            {
                using (CMSEntities cm= new CMSEntities())
                {
                    //you should check duplicate registration here 
                    cm.users.Add(u);
                    cm.SaveChanges();
                    ModelState.Clear();
                    u= null;
                    ViewBag.Message = "Successfully Registration Done";
                }
            }
            return View(u);
        }
    }
}