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
        public ActionResult Index(string search)
        {
            try
            {
                if (Session["LogedUserID"] != null)
                {
                    int userId = int.Parse(Session["LogedUserID"].ToString());

                    var postList = (from p in database.Posts where p.user_id == userId select p)/*.ToList()*/;

                    if (!String.IsNullOrEmpty(search))
                    {
                        postList = postList.Where(s => s.title.Contains(search));
                    }
                    return View(postList.ToList());
                }
                
                return View();
            }

            catch (Exception e)
            {
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
            var getCatergoryList = database.Categories.ToList();
            ViewBag.list = new SelectList(database.Categories, "category_id", "category_name").Distinct();
            
            Post post = new Post();

            return View(post);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Post createPost, FormCollection form, HttpPostedFileBase imageOfPost)
        {
            if (Session["LogedUserID"] != null)
            {
                if (ModelState.IsValid)
                {
                    using (CMSEntities cm = new CMSEntities())
                    {
                        user userId = new user();
                        Post post = new Post();

                        int loginId = int.Parse(Session["LogedUserID"].ToString());
                        userId = database.users.Find(loginId);

                        string catergoryIdInString = form["list"].ToString();
                        int catergoryId = int.Parse(catergoryIdInString);
                        //int categoryId = 2;
                        string status = "Requested";

                        //string catergory = createPost.Category.category_name;
                        var postList = (from p in database.Posts select p.post_id).ToList();

                        //Calculating the post id
                        if (!database.Posts.Any())
                        {
                            int firstPostId = 1;
                            createPost.post_id = firstPostId;
                        }
                        else
                        {
                            int maxPostId = database.Posts.Max(model => model.post_id);
                            int newPostId = maxPostId + 1;
                            createPost.post_id = newPostId;
                        }

                        //if (imageOfPost != null)
                        //{
                        //    //To save images
                        //    post.image = new byte[imageOfPost.ContentLength];
                        //    imageOfPost.InputStream.Read(post.image, 0, imageOfPost.ContentLength);

                        //    string pic = System.IO.Path.GetFileName(imageOfPost.FileName);
                        //    string path = System.IO.Path.Combine(
                        //                           Server.MapPath("E:\\Project\\AccessControlManagement\\PostImages"), pic);
                        //    // file is uploaded
                        //    imageOfPost.SaveAs(path);

                        //    // save the image path path to the database or you can send image 
                        //    // directly to database
                        //    // in-case if you want to store byte[] ie. for DB
                        //    using (MemoryStream ms = new MemoryStream())
                        //    {
                        //        imageOfPost.InputStream.CopyTo(ms);
                        //        byte[] array = ms.GetBuffer();
                        //    }
                        //}
                        

                        post.post_id = createPost.post_id;
                        post.user_id = userId.user_id;
                        post.post_date = createPost.post_date;
                        post.category_id = catergoryId;
                        post.post_description = createPost.post_description;
                        post.activity_log = status;
                        post.title = createPost.title;

                        database.Posts.Add(post);
                        database.SaveChanges();

                        TempData["Success"] = "Post has been created successfully!";
                        
                        return RedirectToAction("Index");
                    }

                    

                }
                
            }

            return View("Create");
        }

        /// <summary>
        /// Method for Viewing Details page of Posts
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns>It returns a view page</returns>
        /// Created by Umatharsini-M
        /// Date - 13/10/2016
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

        [HttpGet]
        public ActionResult Home()
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

                //To display the comments for posts
                var commentlist = (from c in database.Comments select c).ToList();

                List<Object> myModel = new List<object>();
                myModel.Add(postList);
                myModel.Add(commentlist);

                return View(myModel);
            }

            return View();
        }

        //[HttpPost]
        //public ActionResult Home(Comment commentPost, FormCollection form)
        //{
        //    user userId = new user();
        //    Post post = new Post();

        //    int loginId = int.Parse(Session["LogedUserID"].ToString());
        //    userId = database.users.Find(loginId);
        //}

        public ActionResult MyPost()
        {
            if (Session["LogedUserID"] != null)
            {
                int userId = int.Parse(Session["LogedUserID"].ToString());
                var postList = (from p in database.Posts where (p.activity_log.Equals("Accepted")) && (p.user_id==userId) orderby p.post_id ascending select p).ToList();
                return View(postList);
            }
            return View();
        }

    }
}