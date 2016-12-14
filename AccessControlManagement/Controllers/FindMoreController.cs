using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using AccessControlManagement.Models;
using System.Net;
using System.Globalization;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace AccessControlManagement.Controllers
{
    public class FindMoreController : Controller
    {
        public CMSEntities database = new CMSEntities();

        // GET: FindMore
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["LogedUserID"] != null)
            {
                //To display current username
                user user = new user();
                int loginId = int.Parse(Session["LogedUserID"].ToString());
                user = database.users.Find(loginId);
                TempData["User"] = user.username;

                //To display the posts
                var postList = (from p in database.Posts where p.activity_log.Equals("Accepted") orderby p.post_id ascending select p).ToList();

                ////To display the comments for posts
                //var commentlist = (from c in database.Comments select c).ToList();

                //List<Object> myModel = new List<object>();
                //myModel.Add(postList);
                //myModel.Add(commentlist);

                return View(postList);
            }

            return View();
        }
    }
}