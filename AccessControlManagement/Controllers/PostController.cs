using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using AccessControlManagement.Models;
using System.Net;

namespace AccessControlManagement.Controllers
{
    public class PostController : Controller
    {
        public CMSEntities database = new CMSEntities();

        /// <summary>
        /// To View the History of Posts
        /// </summary>
        /// <returns>It returns a History view page</returns>
        /// Created by Umatharsini-M
        /// Date - 9/10/2016
        public ActionResult Index()
        {
            try
            {
                int userId = 2;
                var postList = (from p in database.Posts where p.user_id == userId select p).ToList();

                List<object> myModel = new List<object>();
                myModel.Add(postList);

                return View(postList);
            }

            catch (Exception e)
            {
                ViewBag.Error = "Yes";
                //AddToErrorFile("Post", "Create", e, "");
                return View(e);
            }
        }

        /// <summary>
        /// Method for Viewing Create page of Posts
        /// </summary>
        /// <returns>It returns a view page</returns>
        /// Created by Umatharsini-M
        /// Date - 9/10/2016
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Post post = database.Posts.SingleOrDefault(m => m.post_id == id);

            if (post == null)
            {
                return HttpNotFound();
            }

            return View(post);            
        }
    }
}