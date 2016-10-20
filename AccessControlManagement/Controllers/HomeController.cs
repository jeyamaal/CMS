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
using System.IO;
using System.Web.UI;
using System.Drawing;
using System.Drawing.Imaging;

namespace AccessControlManagement.Controllers
{
    public class HomeController : Controller
    {

        private CMSEntities db = new CMSEntities();
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

       
        /// <summary>
        /// Checking Login Credentials
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        [HttpPost]
      //  [ValidateAntiForgeryToken]
        public ActionResult Login(string un,string pwd)
        {
            try
            {

                if (ModelState.IsValid)
                {
                   
                        var v = db.users.Where(a => a.username.Equals(un) && a.password.Equals(pwd) && a.status.Equals("active")).FirstOrDefault();

                        if (v != null)

                        {
                            Session["LogedUserID"] = v.user_id.ToString();
                            Session["LogedUserFullname"] = v.username.ToString();
                            return RedirectToAction("AfterLogin");
                        }

                        else if(v==null)
                        {
                            return Json("WrongCredits");

                        }
                    
                }
                return RedirectToAction("AfterLogin");

            }
            catch (Exception ex) {
                return Json("WrongCredits");
            }

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
                   using (CMSEntities cm = new CMSEntities())
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

                        }catch (Exception ex)
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


        [HttpPost]
        public ActionResult RecoverPassword(string email )
        {
          try{
                var user = (from u1 in db.users
                            where u1.email_id == email
                            select u1).ToList(); // shangavi created

                if (user != null) // shangavi changed v to user
                {
                    var emailId = db.users.Where(a => a.email_id.Equals(email)).FirstOrDefault();
                    string pwd = emailId.password;

                    if (ModelState.IsValid)
                    {
                        MailMessage mail = new MailMessage();
                        mail.To.Add(email);
                        TempData["notice"] = "Check your mail password is" + "  " +pwd;
                        mail.From = new MailAddress("sender@outlook.com");
                        mail.Subject = "Recover Password";
                        string Body = "Your password is" + "  " +pwd;
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
                        
                        return View("Login");
                    }
                 }

                else
                {
                   return Json("WrongEmail");
                }
            }
            catch (Exception ex) {
                   return Json("WrongEmail");
             }

            return Json("WrongEmail");

        }

        public ActionResult ChangeProfilePicture()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangeProfilePicture(HttpPostedFileBase file, user u)
        {
           try
            {
                user us = new user();
                string db_path = null;
                int ii = int.Parse(Session["LogedUserID"].ToString());
                us = db.users.Find(ii);

                if (file.ContentLength > 0 && file.ContentType.Contains("image"))
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var files = Path.GetExtension(".jpg");

                    if (files != null)
                    {

                        using (var img = Image.FromStream(file.InputStream))
                        {

                            if (img.RawFormat.Equals(ImageFormat.Png) || img.RawFormat.Equals(ImageFormat.Jpeg))
                            {

                                string filestring = fileName.ToString();
                                var path = Path.Combine(Server.MapPath("/Resources/jeyamaal_images"), fileName);
                                file.SaveAs(path);
                                string absoulte_path = path.ToString();
                                db_path = "\\" + GetRightPartOfPath(absoulte_path, "Resources") + "\\" + filestring;
                            }

                            else
                            {
                                return JavaScript("<script>alert(\"Invalidformat\")</script>");

                            }

                        }


                     }

                    u.picture = db_path;
                    us.picture = u.picture;
                    db.Entry(us).State = EntityState.Modified;
                    db.SaveChanges();

                }

                ViewBag.Message = "Upload successful";
                return RedirectToAction("AfterLogin");
            }
            catch (Exception ex)
            {
                //ViewBag.Message = "Upload failed";
                //return RedirectToAction("Index");
                return JavaScript("<script>alert(\"Invalidformat\")</script>");
            }


        }

        private static string GetRightPartOfPath(string path, string startAfterPart)
        {
            // use the correct seperator for the environment
            var pathParts = path.Split(Path.DirectorySeparatorChar);

            // this assumes a case sensitive check. If you don't want this, you may want to loop through the pathParts looking
            // for your "startAfterPath" with a StringComparison.OrdinalIgnoreCase check instead
            int startAfter = Array.IndexOf(pathParts, startAfterPart);

            if (startAfter == -1)
            {
                // path path not found
                return null;
            }

            // try and work out if last part was a directory - if not, drop the last part as we don't want the filename
            var lastPartWasDirectory = pathParts[pathParts.Length - 1].EndsWith(Path.DirectorySeparatorChar.ToString());
            return string.Join(
                Path.DirectorySeparatorChar.ToString(),
                pathParts, startAfter,
                pathParts.Length - startAfter - (lastPartWasDirectory ? 0 : 1));
        }


        public ActionResult DeactiveAccount()
        {
            try
            {
                user us = new user();
                int ii = int.Parse(Session["LogedUserID"].ToString());
                us = db.users.Find(ii);
                us.status = "deactive";
                db.Entry(us).State = EntityState.Modified;
                db.SaveChanges();
                Session.Abandon(); // it will clear the session at the end of request
                return RedirectToAction("Login", "Home");
            }

            catch (Exception ex)
            {
               
            }

            return View();
        }






    }
}