using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccessControlManagement.Models;
using AccessControlManagement.Controllers;
using System.Data.Entity;
using System.Web.Security;
using System.Net.Mail;

namespace AccessControlManagement.Controllers
{
    public class HomeController : Controller
    {

        private CMSEntity db = new CMSEntity();
/**
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
*/

        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(user u)
        {
            // this action is for handle post (login)
            if (ModelState.IsValid) // this is check validity
            {
                using (CMSEntity cm= new CMSEntity())
                {
                    var v = cm.users.Where(a => a.username.Equals(u.username) && a.password.Equals(u.password)).FirstOrDefault();
                    if (v != null)
                    {


                        Session["LogedUserID"] = v.user_id.ToString();
                        Session["LogedUserFullname"] = v.username.ToString();
                        return RedirectToAction("AfterLogin");
                    }

                    else
                    {
                        TempData["Message"] = "Check user name or password";
                        return View(u);
                    }


                }
            }
            return View(u);
        }
      

        public ActionResult AfterLogin()
        {

           
          if (Session["LogedUserID"] != null)
            {
               int  i =  int.Parse(Session["LogedUserID"].ToString());

               user u = db.users.Find(i);

                return View(u);
            }
            else
            {
                return RedirectToAction("Login");
            }
        }



       


        public ActionResult ChangePassword(user u)
        {
            if (Session["LogedUserID"] != null)

            {

              if (ModelState.IsValid) // this is check validity
                {

                using (CMSEntity cm = new CMSEntity())
                {
                    user us = new user();

                  
                       
                      try
                        {
                            int ii = int.Parse(Session["LogedUserID"].ToString());
                            us = db.users.Find(ii);

                       


                            if (u.password==us.password && u.ConfirmPassword==u.newPassword)
                            {

                                string s;

                                s = u.newPassword.ToString();

                                us.password = u.newPassword;


                                db.Entry(us).State = EntityState.Modified;
                                db.SaveChanges();
                              
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
                return RedirectToAction("Login");
            }

            return View();

        }



        public ActionResult logout()
        {

           
            Session.Abandon(); // it will clear the session at the end of request
            
            return RedirectToAction("Login", "Home");

        }

        public ActionResult RecoverPassword(user u)
        {

            if (ModelState.IsValid) // this is check validity
            {
                using (CMSEntity cm = new CMSEntity())
                {
                    var v = cm.users.Where(a => a.email_id.Equals(u.email_id)).FirstOrDefault();

                    if (v != null)
                    {

                        var email = cm.users.Where(a => a.email_id.Equals(u.email_id)).FirstOrDefault();

                        string pwd = email.password;

                        try
                        {
                            if (ModelState.IsValid)
                            {
                                MailMessage mail = new MailMessage();
                                mail.To.Add(u.email_id);
                                TempData["notice"] = "Check your mail password is" + "  " + pwd.ToString();
                                mail.From = new MailAddress("sender@outlook.com");
                                mail.Subject = "Recover Password";
                                string Body = "Your password is" + "  " + pwd.ToString();
                                mail.Body = Body;
                                mail.IsBodyHtml = true;
                                SmtpClient smtp = new SmtpClient();
                                smtp.Host = "smtp.gmail.com";
                                smtp.Port = 587;
                                smtp.UseDefaultCredentials = false;
                                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                                smtp.Credentials = new System.Net.NetworkCredential("ranjaraman5", "ranja123");// Enter seders User name and password
                                smtp.EnableSsl = true;
                                smtp.Send(mail);

                                TempData["notice1"] = "Sucessfully Send";

                                return View("Login");



                            }


                        }
                        catch (Exception ex)
                        {

                        }

                        TempData["notice"] = "Check your mail password is" + pwd;
                        return View(u);

                    }

                    else
                    {
                        TempData["Message"] = "Invalid Email Address";
                        return View(u);
                    }


                }
            }



            return View(u);
        }
    }
}