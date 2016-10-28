using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AccessControlManagement.Models;

namespace AccessControlManagement.Controllers
{
    public class CategoriesController : Controller
    {
        private CMSEntities db = new CMSEntities();
        private object from;



        // GET: Categories
        public ActionResult Index()
        {
            //    List<object> myModel = new List<object>();
            //    myModel.Add(db.Categories.ToList());
            //    myModel.Add(db.Posts.ToList());
            //    //myModel.Add(db.ArticleHasAds.ToList());

            if (Session["LogedUserID"] != null)
            {


                return View("Index");

            }

            else
            {

                return RedirectToAction("Login","Home");
            }
        }

        // GET: Categories
        public ActionResult _Setting()
        {
            List<object> myModel = new List<object>();
            myModel.Add(db.Categories.ToList());
            myModel.Add(db.Posts.ToList());
            //myModel.Add(db.ArticleHasAds.ToList());

            return PartialView(myModel);
        }

        public ActionResult _Post() {
            List<object> postList = new List<object>();

            var groupedUsers = from p in db.Posts
                               group p by new
                               {
                                   p.user_id
                               } into p1
                               select new
                               {
                                   userid = p1.Key.user_id,
                                   NoOfPosts = p1.Select(x => x.post_id).Count()
                               };

            var users = (from u in db.users
                         join gu in groupedUsers on u.user_id equals gu.userid
                         select new
                         {
                             firstName = u.fullname,
                             userName = u.username,
                             email = u.email_id,
                             //role = u.role,
                             postCount = gu.NoOfPosts
                         }).ToList();

            postList.Add(db.users.ToList());

            return PartialView(postList);
        }

        public ActionResult AddNewCategory(string category_name)
        {
            try
            {
                var id = (from c in db.Categories
                          select c.category_id).ToList();

                var countID = id.Count();

                if (countID <= 0)
                {
                    countID = countID + 1;
                    int resultInsertCategory = db.usp_Category_insert(countID, category_name);

                    if (resultInsertCategory == 1)
                    {
                        return Content("Success");
                    }
                    else
                    {
                        return Content("Not Inserted");
                    }
                }
                else
                {
                    return Content("Number of count");
                }
                
            }
            catch
            {
                return Content("Error");
            }
        }
        
        [HttpPost]
        public ActionResult Update(string oldCategoryName, string newCategoryName)
        {
            try
            {
                int resultUpdateCategory = db.usp_Category_update(oldCategoryName, newCategoryName);

                if (resultUpdateCategory == 1)
                {
                    return Content("Success");
                }
                else
                {
                    return Content("Not Success");
                }
            }
            catch
            {
                return Content("Error to load the Procedure");
            }
        }

        [HttpPost]
        public ActionResult GetCategoryName(int categoryID)
        {
            try
            {
                var categoryName = (from c in db.Categories
                                    where c.category_id == categoryID
                                    select c).ToList();

                return Json(categoryName[0].category_name.ToString(), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Not Such Value Exists", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteCategory(string category) {
            try
            {
                int resultDeleteCategory = db.usp_Category_delete(category);

                if (resultDeleteCategory == 1)
                {
                    return Content("Success");
                }
                else
                {
                    return Content("Not Success");
                }
            }

            catch (Exception e)
            {
                return Content("Error to load the Procedure");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
