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
using System.Diagnostics;

namespace AccessControlManagement.Controllers
{
    public class HomeController : Controller
    {
        
        private CMSEntities db = new CMSEntities();

        /// <summary>
        /// To View Profile
        /// </summary>
        /// <returns>It returns Profile of the User</returns>
        /// Created by- Jeyamaal
        /// Date-9/10/2016

        public ActionResult Index()
        {
            return View();
        }


        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}



        /// <summary>
        /// To View Profile
        /// </summary>
        /// <returns>It returns Profile of the User</returns>
        /// Created by- Jeyamaal
        /// Date-9/9/2016
        public ActionResult ProfileView()
        {
            if (Session["LogedAdminID"] != null)
            {

                int i = int.Parse(Session["LogedAdminID"].ToString());

                user u = db.users.Find(i);
                return PartialView(u);
            }


            else if (Session["LogedUserID"] != null)
            {

                int i = int.Parse(Session["LogedUserID"].ToString());

                user u = db.users.Find(i);
                return View(u);
            }


            else if (Session["LogedAdevertiserID"] != null)
            {

                int i = int.Parse(Session["LogedAdevertiserID"].ToString());

                user u = db.users.Find(i);
                return View(u);
            }


            else
            {
                return View("Login");

            }

        }



        /// <summary>
        /// To Login to the System via GetMethod
        /// </summary>
        /// <returns>It returns the Login page</returns>
        /// Created by- Jeyamaal
        /// Date-13/9/2016

        public ActionResult Login()
        {
            return View();
        }



        /// <summary>
        /// To Login to the System via GetMethod
        /// </summary>
        /// <param name="un">username of the user</param>
        /// <param name="pwd">password of the user</param>
        /// <returns>Return page according to the credentials</returns>
        /// Created by- Jeyamaal
        /// Date-13/9/2016

        [HttpPost]
      //[ValidateAntiForgeryToken]
        public ActionResult Login(string un, string pwd)
        {
            try
            {

                if (ModelState.IsValid)
                {

                    var v = db.users.Where(a => a.username.Equals(un) && a.password.Equals(pwd) && a.status.Equals("active")).FirstOrDefault();

                    string vv = v.role.ToString();

                    if (v != null)

                    {

                        string vv2 = v.role.ToString();

                        if (v.role.ToString() == "writer")
                        {
                            //Jeyamaal

                            //Session["LogedUserID"] = v.user_id.ToString();
                             Session["LogedUserFullname"] = v.username.ToString();
                            //return RedirectToAction("AfterLogin");

                            Session["LogedUserID"] = v.user_id.ToString();
                          
                            return RedirectToAction("Index","Post");

                        }


                        else if (v.role.ToString() == "admin")
                        {
                            Session["LogedAdminID"] = v.user_id.ToString();
                            Session["LogedUserFullname"] = v.username.ToString();
                            return RedirectToAction("Index", "Categories");

                        }

                        else if (v.role.ToString() == "advertiser")
                        {
                            Session["LogedAdevertiserID"] = v.user_id.ToString();
                            Session["LogedUserFullname"] = v.username.ToString();
                            return RedirectToAction("Index", "Advertisement");

                        }




                    }

                    else if (v == null)
                    {
                        return Json("WrongCredits");

                    }
                }


            }
            catch (Exception ex)
            {
                return Json("WrongCredits");
            }

            return RedirectToAction("Index", "Categories");
        }



        /// <summary>
        /// To redirect the page acccording to the correct credentials entered by the user
        /// </summary>
        /// <returns>User's main page (Admin/Writer/User)</returns>
        /// Created by- Jeyamaal
        /// Date-17/9/2016
        public ActionResult AfterLogin()
        {


            if (Session["LogedUserID"] != null)
            {
                //int i = int.Parse(Session["LogedUserID"].ToString());
                //user u = db.users.Find(i);
                //return View(u);
                return RedirectToAction("Index", "Post");
            }

            else if (Session["LogedAdminID"] != null)
            {
             
                return RedirectToAction("Index", "Categories");

            }


            else if (Session["LogedAdevertiserID"] != null)
            {

                return RedirectToAction("Index", "Advertisement");

            }

            else
            {
                return RedirectToAction("Login");
            }
        }

        /// <summary>
        /// Reset the current password
        /// </summary>
        /// <param name="oldPwd">Old password</param>
        /// <param name="conPwd">Confirm the old password</param>
        /// <param name="newPwd">new password</param>
        /// <returns>Change password Modal(HTML Modal)</returns>
        /// Created by- Jeyamaal
        /// Date-23/9/2016
        public ActionResult ChangePassword(string oldPwd, string conPwd,string newPwd)
       {
       if (Session["LogedAdminID"] != null)
       {

          try{
                if (ModelState.IsValid) // this is check validity
                 {
                        Debug.WriteLine(oldPwd);
                        Debug.WriteLine(conPwd);
                        Debug.WriteLine(newPwd);

                        user us = new user();
                        int ii = int.Parse(Session["LogedAdminID"].ToString());
                        us = db.users.Find(ii);
                        if ( us.password==oldPwd && conPwd==newPwd)
                        {

                            us.password = newPwd;
                            db.Entry(us).State = EntityState.Modified;
                            db.SaveChanges();
                            Session.Abandon(); // it will clear the session at the end of request
                            return RedirectToAction("Login", "Home");
                        
                        }
                  }

                }catch (Exception ex)
                {
                        return Json("WrongChangePassword");
                }
                    
         }
         else
         {
                return Json("WrongChangePassword");
          }

            return Json("WrongChangePassword");
        }


        /// <summary>
        /// Logout from the system
        /// </summary>
        /// <returns>It returns the Login page </returns>
        /// Created by- Jeyamaal
        /// Date-1/10/2016
        public ActionResult logout()
        {
             Session.Abandon(); // it will clear the session at the end of request
             return RedirectToAction("Login", "Home");
        }





        /// <summary>
        /// Recover the password when user forget the password.
        /// The new password is sent by the system to user's email address
        /// </summary>
        /// <param name="email">User's email address</param>
        /// <returns></returns>
        /// Created by- Jeyamaal
        /// Date-5/10/2016

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
        /// <summary>
        /// Change the profile picture of the user via Get method
        /// </summary>
        /// <returns>It returns the Modal of the "Change Picture page"</returns>
        /// Created by- Jeyamaal
        /// Date-5/10/2016
        public ActionResult ChangeProfilePicture()
        {
            return View();
        }


        /// <summary>
        /// Change the profile picture of the user via Post method
        /// user has to input image file.
        /// </summary>
        /// <param name="file">Image file</param>
        /// <param name="u">user details</param>
        /// <returns>It return the Modal of the "Change Picture page"</returns>
        /// Created by- Jeyamaal
        /// Date-19/10/2016
        [HttpPost]
        public ActionResult ChangeProfilePicture(HttpPostedFileBase file, user u)
        {
           try
            {
                user us = new user();
                string db_path = null;
                int ii = int.Parse(Session["LogedAdminID"].ToString());
                us = db.users.Find(ii);

                if (file.ContentLength > 0 && file.ContentType.Contains("image"))
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var files = Path.GetExtension(".jpg");

                    if (files != null)
                    {

                        var img = Image.FromStream(file.InputStream);


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
                            TempData["Message1"] = "Profile Updated Successfully";
                            return JavaScript("<script>alert(\"Invalidformat\")</script>");

                        }




                    }

                    u.picture = db_path;
                    us.picture = u.picture;
                    db.Entry(us).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["SucessImageUpload"] = "Success_Image";
                    return RedirectToAction("ProfileView");

                }


                else
                {

                    TempData["Message"] = "UnSuccess";
                    return RedirectToAction("ProfileView");

                }

            }
            catch (Exception ex)
            {
                
                TempData["Message"] = "UnSuccess";
                return RedirectToAction("ProfileView");
            }


        }
        /// <summary>
        /// To Get the file extension of the File
        /// </summary>
        /// <param name="path">Full path of the file</param>
        /// <param name="startAfterPart">Where we want to  extract from the path name</param>
        /// <returns>It returs the customize file path</returns>
        /// Created by- Jeyamaal
        /// Date-19/10/2016
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


        /// <summary>
        /// Deactivate the user's account by the user
        /// </summary>
        /// <returns>It returns Confirmation message,according to the users response it will deactivavte the account  or not</returns>
        /// Created by- Jeyamaal
        /// Date-26/10/2016
        public ActionResult DeactiveAccount()
        {
            try
            {
                user us = new user();
                int ii = int.Parse(Session["LogedAdminID"].ToString());
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

        /// <summary>
        /// It will show the graph of the the No of the posts orederd by month 
        /// </summary>
        /// <returns>It returns the NOOfPostsGraph.cshtml</returns>
        /// Created by- Jeyamaal
        /// Date-26/10/2016
        public ActionResult NoOfPostsGraph()
        {
            if (Session["LogedAdminID"] != null)
            {
                user us;
                int ii = int.Parse(Session["LogedAdminID"].ToString());
                us = db.users.Find(ii);

                return View(us);
            }

            return View();
        }


        /// <summary>
        /// It will show the graph of the AccountStatus (Number of Active and Deactive users) 
        /// </summary>
        /// <returns>It returns the AccountStatusGraph.cshtml</returns>
        /// Created by- Jeyamaal
        /// Date-26/10/2016
        public ActionResult AccountStatusGraph()
        {
            if (Session["LogedAdminID"] != null)
            {
                user us;
                int ii = int.Parse(Session["LogedAdminID"].ToString());
                us = db.users.Find(ii);

                return View(us);
            }

            return View();
        }

        /// <summary>
        /// It will show the graph of the AdvertisementStatus (Number of advertisements posted/pending/accept) 
        /// </summary>
        /// <returns>It returns the AdvertisementStatusGraph.cshtml</returns>
        /// Created by- Jeyamaal
        /// Date-26/10/2016
        public ActionResult AdvertisementStatusGraph()
        {
            if (Session["LogedAdminID"] != null)
            {
                user us;
                int ii = int.Parse(Session["LogedAdminID"].ToString());
                us = db.users.Find(ii);

                return View(us);
            }

            return View();
        }






        /// <summary>
        /// To send the feedback as a text to system
        /// </summary>
        /// <param name="u">user details</param>
        /// <returns>It return the Modal of the Feedback page</returns>
        /// Created by- Jeyamaal
        /// Date-10/11/2016
        [HttpPost]
 
        public ActionResult SendFeedBack(user u)
        {
            Feedback fd = new Feedback();
            try
            {
                if (ModelState.IsValid)
                {
                    fd.firstname = u.fullname.ToString(); ;
                    fd.email = u.email_id.ToString();
                    fd.description = u.feedbacks.ToString();
                    fd.feed_date = DateTime.Now;
                    db.Feedbacks.Add(fd);
                    db.SaveChanges();
                    ModelState.Clear();
                    fd = null;

                    TempData["SucessFeedbackMessage"] = "Success";
                    return RedirectToAction("ProfileView");

                   }
                }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
                TempData["ErrorFeedbackMessage"] = "UnSuccess";
                return RedirectToAction("ProfileView");
            }

            return RedirectToAction("ProfileView");
        }


    }
}