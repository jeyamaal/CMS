﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccessControlManagement.Models;


namespace AccessControlManagement.Controllers
{
    public class RegisterController : Controller
    {

        public ActionResult Register()
        {
            return View();
        }



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