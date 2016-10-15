using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccessControlManagement.Models;
using System.Data.Entity;

namespace AccessControlManagement.Controllers
{
    public class ProfileController : Controller
    {
        CMSEntities cm = new CMSEntities();

        // GET: Profile
        public ActionResult Index()
        {
            return View();
        }



        public ActionResult ChangePassword(user u)
        {
            if (Session["LogedUserID"] != null)

            {

                if (ModelState.IsValid) // this is check validity
                {

                    using (CMSEntities cm = new CMSEntities())
                    {
                        user us = new user();



                        try
                        {
                            int ii = int.Parse(Session["LogedUserID"].ToString());
                            us = cm.users.Find(ii);




                            if (u.password == us.password && u.ConfirmPassword == u.newPassword)
                            {

                                string s;

                                s = u.newPassword.ToString();

                                us.password = u.newPassword;


                                cm.Entry(us).State = EntityState.Modified;
                                cm.SaveChanges();

                                // TempData["notice"] = "Successfully changed" + us.password.ToString() + ii;

                                return View("ChangePassword");

                            }

                            else
                            {
                                TempData["notice"] = "Successfully changed" + us.password.ToString() + ii;

                            }


                        }

                        catch (Exception ex)
                        {

                        }
                     }
                }
            }

            else
            {
                return RedirectToAction("Login","Home");
            }

            return View();

        }






    }
}