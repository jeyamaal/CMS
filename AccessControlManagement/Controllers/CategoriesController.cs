using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AccessControlManagement.Models;
using System.Diagnostics;

namespace AccessControlManagement.Controllers
{
    public class CategoriesController : Controller
    {
        private CMSEntities db = new CMSEntities();
      
        // GET: Categories
        public ActionResult Index()
        {
            if (Session["LogedAdminID"] != null)
            {
                List<object> postList = new List<object>();

                ViewBag.UsersNameList = new SelectList(db.users.Where(t => (t.status.Equals("active")) && (t.role.Equals("writer"))), "username", "username").Distinct();

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
                             select new CategoryUsers
                             {
                                 firstName = u.fullname,
                                 userName = u.username,
                                 email = u.email_id,
                                 myrole = u.role.ToString(),
                                 postCount = gu.NoOfPosts
                             });


                var advertisments = (from ad in db.AdvertisementDetails
                                     join c in db.Categories on ad.category_id equals c.category_id
                                     select new AdvertisementCategory
                                     {
                                         adID = ad.ADD_id,
                                         categoryName = c.category_name,
                                         postedDate = ad.postedDate.ToString(),
                                         title = ad.title,
                                         status = ad.status,
                                         expirayDate = ad.dueDate.ToString()
                                     });
             
                int i = int.Parse(Session["LogedAdminID"].ToString());
                //user u1 = db.users.Find(i);

                var usermm = (from c in db.users
                              where c.user_id == i
                              select c).ToList();
                postList.Add(usermm);
                postList.Add(users.ToList());
                postList.Add(advertisments.ToList());
        
                return View(postList);
            }

            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        //public ActionResult ChangeAdStatus(int adID, string statusAD)
        //{
        //    //try
        //    //{
        //    //    int resultStatusUpdate = db.usp_Advertisement_statusUpdate(adID, statusAD);
        //    //    Debug.WriteLine("Advetisement update" + resultStatusUpdate);
        //    //    if (resultStatusUpdate == 1)
        //    //    {
        //    //        return Json("Status is successfully Changed!", JsonRequestBehavior.AllowGet);
        //    //    }
        //    //    else
        //    //    {
        //    //        return Json("Status is not Changed!", JsonRequestBehavior.AllowGet);
        //    //    }
        //    //}
        //    //catch
        //    //{
        //    //    return Json("Failed To Update!!", JsonRequestBehavior.AllowGet);
        //    //}
        //}
        // GET: Categories
        public ActionResult _Setting()
        {
            List<object> myModel = new List<object>();
            myModel.Add(db.Categories.ToList());
            myModel.Add(db.Posts.ToList());

            return PartialView(myModel);
        }

        //public ActionResult _Post()
        //{

        //}

        public ActionResult GetDropDownValueUser(string user_name)
        {
            try
            {
                var postDetails = (from u in db.users
                                   join p in db.Posts on u.user_id equals p.user_id 
                                   where u.username == user_name
                                   select new
                                   {
                                       p.Category.category_name,
                                       p.post_date,
                                       p.title
                                   });

                if ((postDetails.ToList()) != null)
                {
                    return Json(postDetails.ToList(), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("No post has been written by " + user_name, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json("Error while retrieving post Details", JsonRequestBehavior.AllowGet);
            }
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
