using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccessControlManagement.Models;


namespace AccessControlManagement.Controllers
{
    public class RegisterController : Controller
    {
        /// <summary>
        ///To get the Register page via Get method 
        ///
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Pass the registered user deatils to database
        /// </summary>
        /// <param name="u">User details entered by user</param>
        /// <returns>It return the Register page</returns>

        // GET: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(user u)
        {
            if (ModelState.IsValid)
            {
                using (CMSEntities cm = new CMSEntities())
                {
                if  (cm.users.Any(a => a.email_id.Equals(u.email_id) && a.username.Equals(u.username) && a.fullname.Equals(u.fullname)))
                     {
                        ViewBag.Message = "Already Registration Done";
                    }

                    else
                    {
                        var myrole = u.role.ToString();
                        u.status = "active";
                        cm.users.Add(u);
                        cm.SaveChanges();
                        ModelState.Clear();
                        u = null;
                        ViewBag.Message = "Successfully Registration Done";
                    }
              
                }
            }
            return View(u);
        }
    }
}