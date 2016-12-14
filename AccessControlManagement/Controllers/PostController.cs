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

            if (Session["LogedUserID"] != null)
            {
                try
                {
                    int userId = int.Parse(Session["LogedUserID"].ToString());

                    List<object> postAccessList = new List<object>();

                    var postDetails = (from c in database.Posts

                                       select new PostAccess
                                       {
                                           post_id = c.post_id,
                                           user_id = c.user_id,
                                           title = c.title,
                                           category_id = c.category_id,
                                           post_description = c.post_description,
                                           post_date = c.post_date,
                                           activity_log = c.activity_log,
                                           image = c.image,
                                           Category = c.Category

                                       });
                    var postList = (from p in database.Posts where p.user_id == userId select p)/*.ToList()*/;

                    

                    if (!String.IsNullOrEmpty(search))
                    {
                        postDetails = postDetails.Where(s => s.title.Contains(search));
                    }

                    var myPost = postDetails.ToList();
                    var usermm = (from c in database.users
                                  where c.user_id == userId
                                  select c).ToList();

                    string[] filePaths = Directory.GetFiles(Server.MapPath("~/Resources/Advetrisement_Image/"));
                    List<AdvertisementDetail> files = new List<AdvertisementDetail>();
                    foreach (string filePath in filePaths)
                    {
                        string fileName = Path.GetFileName(filePath);
                        files.Add(new AdvertisementDetail
                        {

                            title = fileName.Split('.')[0].ToString(),
                            adImage = "../Resources/Advetrisement_Image/" + fileName
                        });
                    }


                    postAccessList.Add(usermm);
                    postAccessList.Add(myPost);
                    postAccessList.Add(files);
                    return View(postAccessList);
                }

                catch (Exception e)
                {
                    return View(e);
                }
            }

            else
            {
                return RedirectToAction("Login", "Home");
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

            //int userId = int.Parse(Session["LogedUserID"].ToString());

            //List<object> postAccessList = new List<object>();

            //var postDetails = (from c in database.Posts

            //                   select new PostAccess
            //                   {
            //                       post_id = c.post_id,
            //                       user_id = c.user_id,
            //                       title = c.title,
            //                       category_id = c.category_id,
            //                       post_description = c.post_description,
            //                       post_date = c.post_date,
            //                       activity_log = c.activity_log,
            //                       image = c.image,
            //                       Category = c.Category

            //                   });


            //var myPost = postDetails.ToList();
            //var usermm = (from c in database.users
            //              where c.user_id == userId
            //              select c).ToList();

            //postAccessList.Add(usermm);
            //postAccessList.Add(myPost);
            //return View(postAccessList);

            Post post = new Post();

            return View(post);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Post createPost, FormCollection form, HttpPostedFileBase file)
        {
            try
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

                            string dbPath = null;

                            HttpPostedFileBase file1 = file;
                            int num = file.ContentLength;
                            bool e = file.ContentType.Contains("image");

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
                                        var path = Path.Combine(Server.MapPath("/Resources/PostImages"), fileName);
                                        file.SaveAs(path);
                                        string absoulte_path = path.ToString();
                                        dbPath = "\\" + GetRightPartOfPath(absoulte_path, "Resources") + "\\" + filestring;

                                        post.post_id = createPost.post_id;
                                        post.user_id = userId.user_id;
                                        post.post_date = createPost.post_date;
                                        post.category_id = catergoryId;
                                        post.post_description = createPost.post_description;
                                        post.image = dbPath;
                                        post.activity_log = status;
                                        post.title = createPost.title;

                                        database.Posts.Add(post);
                                        database.SaveChanges();

                                        TempData["Success"] = "Post has been created successfully!";

                                        return RedirectToAction("Index");
                                    }

                                    else
                                    {
                                        TempData["UnsuccessImage"] = "Image Format should be .png or .jpg";
                                        return RedirectToAction("Create");
                                    }
                                }

                                else
                                {
                                    TempData["NullImage"] = "Image Format should be .png or .jpg11111";
                                    return RedirectToAction("Create");
                                }
                            }

                            else
                            {
                                TempData["UnsuccessNullImage"] = "Not Image";
                                return View("Create");
                            }

                            //post.post_id = createPost.post_id;
                            //post.user_id = userId.user_id;
                            //post.post_date = createPost.post_date;
                            //post.category_id = catergoryId;
                            //post.post_description = createPost.post_description;
                            //post.image = dbPath;
                            //post.activity_log = status;
                            //post.title = createPost.title;

                            //database.Posts.Add(post);
                            //database.SaveChanges();

                            //TempData["Success"] = "Post has been created successfully!";

                            //return RedirectToAction("Index");
                        }



                    }

                }

                return View("Create");
            }

            catch (Exception e)
            {
                TempData["UnsuccessNullImage"] = "Null" + e;
                return View("Create");
            }
        }

        public static string GetRightPartOfPath(string path, string startAfterPart)
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

        [HttpGet]
        public ActionResult Edit(int id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var getCatergoryList = database.Categories.ToList();
            ViewBag.list = new SelectList(database.Categories, "category_id", "category_name").Distinct();

            Post post = database.Posts.Single(p => p.post_id == id);

            if (post == null)
                return HttpNotFound();
            
            return View(post);
        }


        [HttpPost]
        public ActionResult Edit(Post createPost, FormCollection form, HttpPostedFileBase file)
        {
            try
            {
                if (Session["LogedUserID"] != null)
                {
                    if (ModelState.IsValid)
                    {
                        using (CMSEntities cm = new CMSEntities())
                        {
                            int id = createPost.post_id;

                            user userId = new user();
                            Post post = new Post();

                            post = database.Posts.Find(id);

                            int loginId = int.Parse(Session["LogedUserID"].ToString());
                            userId = database.users.Find(loginId);

                            string catergoryIdInString = form["list"].ToString();
                            int catergoryId = int.Parse(catergoryIdInString);

                            string dbPath = null;

                            HttpPostedFileBase file1 = file;
                            int num = file.ContentLength;
                            bool e = file.ContentType.Contains("image");

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
                                        var path = Path.Combine(Server.MapPath("/Resources/PostImages"), fileName);
                                        file.SaveAs(path);
                                        string absoulte_path = path.ToString();
                                        dbPath = "\\" + GetRightPartOfPath(absoulte_path, "Resources") + "\\" + filestring;

                                        post.post_date = createPost.post_date;
                                        post.category_id = catergoryId;
                                        post.post_description = createPost.post_description;
                                        post.image = dbPath;
                                        post.title = createPost.title;

                                        database.Entry(post).State = EntityState.Modified;
                                        database.SaveChanges();
                                        
                                        TempData["EditSuccess"] = "Post has been Updated successfully!";

                                        return RedirectToAction("Index");
                                    }

                                    else
                                    {
                                        TempData["UnsuccessImage"] = "Image Format should be .png or .jpg";
                                        return RedirectToAction("Create");
                                    }
                                }

                                else
                                {
                                    TempData["NullImage"] = "Image Format should be .png or .jpg11111";
                                    return RedirectToAction("Create");
                                }
                            }

                            else
                            {
                                TempData["UnsuccessNullImage"] = "Not Image";
                                return View("Create");
                            }

                            //post.post_id = createPost.post_id;
                            //post.user_id = userId.user_id;
                            //post.post_date = createPost.post_date;
                            //post.category_id = catergoryId;
                            //post.post_description = createPost.post_description;
                            //post.image = dbPath;
                            //post.activity_log = status;
                            //post.title = createPost.title;

                            //database.Posts.Add(post);
                            //database.SaveChanges();

                            //TempData["Success"] = "Post has been created successfully!";

                            //return RedirectToAction("Index");
                        }



                    }

                }

                return View("Create");
            }

            catch (Exception e)
            {
                TempData["UnsuccessNullImage"] = "Null" + e;
                return View("Create");
            }

           
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

                var postList = (from p in database.Posts where p.activity_log.Equals("Accepted") orderby p.post_id ascending select p).ToList();

                var postDetails = (from c in database.Posts
                                   where (c.activity_log.Equals("Accepted")) && (c.user_id == loginId)
                                   orderby c.post_id ascending
                                   select new PostAccess
                                   {
                                       post_id = c.post_id,
                                       user_id = c.user_id,
                                       title = c.title,
                                       category_id = c.category_id,
                                       post_description = c.post_description,
                                       post_date = c.post_date,
                                       activity_log = c.activity_log,
                                       image = c.image,
                                       Category = c.Category,
                                       user = c.user

                                   });
                var myPost = postDetails.ToList();

                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Resources/Advetrisement_Image/"));
                List<AdvertisementDetail> files = new List<AdvertisementDetail>();
                foreach (string filePath in filePaths)
                {
                    string fileName = Path.GetFileName(filePath);
                    files.Add(new AdvertisementDetail
                    {

                        title = fileName.Split('.')[0].ToString(),
                        adImage = "../Resources/Advetrisement_Image/" + fileName
                    });
                }

                int userId = int.Parse(Session["LogedUserID"].ToString());

                var usermm = (from c in database.users
                              where c.user_id == userId
                              select c).ToList();

                List<Object> myModel = new List<object>();
                myModel.Add(usermm);
                myModel.Add(myPost);
                myModel.Add(files);
                

                return View(myModel);


            }

            return View();
        }

        [HttpPost]
        public ActionResult Home(Comment commentPost, FormCollection form)
        {
            if (Session["LogedUserID"] != null)
            {
                if (ModelState.IsValid)
                {
                    using (CMSEntities cm = new CMSEntities())
                    {
                        user user = new user();
                        Comment comment = new Comment();

                        int loginId = int.Parse(Session["LogedUserID"].ToString());
                        user = database.users.Find(loginId);

                        string description = form["txtComment"].ToString();
                        
                       // string postIdInString = form["hdnPostId"].ToString();
                        //int postId = int.Parse(postIdInString);

                        comment.user_id = user.user_id;
                        comment.post_id = commentPost.post_id;
                        comment.description = description;
                        comment.comment_date = DateTime.Now;

                        database.Comments.Add(comment);
                        database.SaveChanges();

                        //TempData["Success"] = "Post has been created successfully!";

                        return RedirectToAction("Home");
                    }
                }


            }
            return View("Create");        
        }

        public ActionResult MyPost()
        {
            if (Session["LogedUserID"] != null)
            {
                user user = new user();
                int loginId = int.Parse(Session["LogedUserID"].ToString());
                user = database.users.Find(loginId);
                TempData["User"] = user.username;

                

                var postDetails = (from c in database.Posts
                                   where (c.activity_log.Equals("Accepted")) && (c.user_id == loginId)
                                   orderby c.post_id ascending
                                   select new PostAccess
                                   {
                                       post_id = c.post_id,
                                       user_id = c.user_id,
                                       title = c.title,
                                       category_id = c.category_id,
                                       post_description = c.post_description,
                                       post_date = c.post_date,
                                       activity_log = c.activity_log,
                                       image = c.image,
                                       Category = c.Category,
                                       user = c.user

                                   });
                var myPost = postDetails.ToList();

                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Resources/Advetrisement_Image/"));
                List<AdvertisementDetail> files = new List<AdvertisementDetail>();
                foreach (string filePath in filePaths)
                {
                    string fileName = Path.GetFileName(filePath);
                    files.Add(new AdvertisementDetail
                    {

                        title = fileName.Split('.')[0].ToString(),
                        adImage = "../Resources/Advetrisement_Image/" + fileName
                    });
                }

                int userId = int.Parse(Session["LogedUserID"].ToString());

                var usermm = (from c in database.users
                              where c.user_id == userId
                              select c).ToList();

                List<Object> myModel = new List<object>();
                myModel.Add(usermm);
                myModel.Add(myPost);
                myModel.Add(files);


                return View(myModel);
            }
            return View();
        }

        public ActionResult Viewww()
        {

            return View("View");

        }


        // GET: /Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Post post = database.Posts.Find(id);

            string status = post.activity_log;
        
            if (post == null)
            {
                return HttpNotFound();
            }

            if (status == "Requested")
            {
                TempData["Delete"] = "You cannot delete the post because the post Status is Requested";
                return RedirectToAction("Index");
            }

            else
            {
                return View(post);
            }
        }

        // POST: /Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = database.Posts.Find(id);

            database.Posts.Remove(post);
            database.SaveChanges();

            TempData["DeleteSuccess"] = "Successfully DELETED";
            
            return RedirectToAction("Index");
        }

        public ActionResult Education()
        {
            if (Session["LogedUserID"] != null)
            {
              
                user user = new user();
                int loginId = int.Parse(Session["LogedUserID"].ToString());
                user = database.users.Find(loginId);
                TempData["User"] = user.username;



                var postDetails = (from c in database.Posts
                                   where (c.activity_log.Equals("Accepted")) && (c.Category.category_name.Equals("Education"))
                                   orderby c.post_id ascending
                                   select new PostAccess
                                   {
                                       post_id = c.post_id,
                                       user_id = c.user_id,
                                       title = c.title,
                                       category_id = c.category_id,
                                       post_description = c.post_description,
                                       post_date = c.post_date,
                                       activity_log = c.activity_log,
                                       image = c.image,
                                       Category = c.Category,
                                       user = c.user

                                   });
                var myPost = postDetails.ToList();

                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Resources/Advetrisement_Image/"));
                List<AdvertisementDetail> files = new List<AdvertisementDetail>();
                foreach (string filePath in filePaths)
                {
                    string fileName = Path.GetFileName(filePath);
                    files.Add(new AdvertisementDetail
                    {

                        title = fileName.Split('.')[0].ToString(),
                        adImage = "../Resources/Advetrisement_Image/" + fileName
                    });
                }

                int userId = int.Parse(Session["LogedUserID"].ToString());

                var usermm = (from c in database.users
                              where c.user_id == userId
                              select c).ToList();

                List<Object> myModel = new List<object>();
                myModel.Add(usermm);
                myModel.Add(myPost);
                myModel.Add(files);


                return View(myModel);
            }

            else
            {
                return View("Index");
            }
        }

        public ActionResult BabyCare()
        {
            if (Session["LogedUserID"] != null)
            {

                user user = new user();
                int loginId = int.Parse(Session["LogedUserID"].ToString());
                user = database.users.Find(loginId);
                TempData["User"] = user.username;



                var postDetails = (from c in database.Posts
                                   where (c.activity_log.Equals("Accepted")) && (c.Category.category_name.Equals("Baby Care"))
                                   orderby c.post_id ascending
                                   select new PostAccess
                                   {
                                       post_id = c.post_id,
                                       user_id = c.user_id,
                                       title = c.title,
                                       category_id = c.category_id,
                                       post_description = c.post_description,
                                       post_date = c.post_date,
                                       activity_log = c.activity_log,
                                       image = c.image,
                                       Category = c.Category,
                                       user = c.user

                                   });
                var myPost = postDetails.ToList();

                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Resources/Advetrisement_Image/"));
                List<AdvertisementDetail> files = new List<AdvertisementDetail>();
                foreach (string filePath in filePaths)
                {
                    string fileName = Path.GetFileName(filePath);
                    files.Add(new AdvertisementDetail
                    {

                        title = fileName.Split('.')[0].ToString(),
                        adImage = "../Resources/Advetrisement_Image/" + fileName
                    });
                }

                int userId = int.Parse(Session["LogedUserID"].ToString());

                var usermm = (from c in database.users
                              where c.user_id == userId
                              select c).ToList();

                List<Object> myModel = new List<object>();
                myModel.Add(usermm);
                myModel.Add(myPost);
                myModel.Add(files);


                return View(myModel);
            }

            else
            {
                return View("Index");
            }
        }

        public ActionResult FoodNutrition()
        {
            if (Session["LogedUserID"] != null)
            {

                user user = new user();
                int loginId = int.Parse(Session["LogedUserID"].ToString());
                user = database.users.Find(loginId);
                TempData["User"] = user.username;



                var postDetails = (from c in database.Posts
                                   where (c.activity_log.Equals("Accepted")) && (c.Category.category_name.Equals("Food & Nutrition"))
                                   orderby c.post_id ascending
                                   select new PostAccess
                                   {
                                       post_id = c.post_id,
                                       user_id = c.user_id,
                                       title = c.title,
                                       category_id = c.category_id,
                                       post_description = c.post_description,
                                       post_date = c.post_date,
                                       activity_log = c.activity_log,
                                       image = c.image,
                                       Category = c.Category,
                                       user = c.user

                                   });
                var myPost = postDetails.ToList();

                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Resources/Advetrisement_Image/"));
                List<AdvertisementDetail> files = new List<AdvertisementDetail>();
                foreach (string filePath in filePaths)
                {
                    string fileName = Path.GetFileName(filePath);
                    files.Add(new AdvertisementDetail
                    {

                        title = fileName.Split('.')[0].ToString(),
                        adImage = "../Resources/Advetrisement_Image/" + fileName
                    });
                }

                int userId = int.Parse(Session["LogedUserID"].ToString());

                var usermm = (from c in database.users
                              where c.user_id == userId
                              select c).ToList();

                List<Object> myModel = new List<object>();
                myModel.Add(usermm);
                myModel.Add(myPost);
                myModel.Add(files);


                return View(myModel);
            }

            else
            {
                return View("Index");
            }
        }

        public ActionResult Others()
        {
             if (Session["LogedUserID"] != null)
            {

                user user = new user();
                int loginId = int.Parse(Session["LogedUserID"].ToString());
                user = database.users.Find(loginId);
                TempData["User"] = user.username;



                var postDetails = (from c in database.Posts
                                   where (c.activity_log.Equals("Accepted")) && (c.Category.category_name.Equals("Science") || c.Category.category_name.Equals("Business Management"))
                                   orderby c.post_id ascending
                                   select new PostAccess
                                   {
                                       post_id = c.post_id,
                                       user_id = c.user_id,
                                       title = c.title,
                                       category_id = c.category_id,
                                       post_description = c.post_description,
                                       post_date = c.post_date,
                                       activity_log = c.activity_log,
                                       image = c.image,
                                       Category = c.Category,
                                       user = c.user

                                   });
                var myPost = postDetails.ToList();

                string[] filePaths = Directory.GetFiles(Server.MapPath("~/Resources/Advetrisement_Image/"));
                List<AdvertisementDetail> files = new List<AdvertisementDetail>();
                foreach (string filePath in filePaths)
                {
                    string fileName = Path.GetFileName(filePath);
                    files.Add(new AdvertisementDetail
                    {

                        title = fileName.Split('.')[0].ToString(),
                        adImage = "../Resources/Advetrisement_Image/" + fileName
                    });
                }

                int userId = int.Parse(Session["LogedUserID"].ToString());

                var usermm = (from c in database.users
                              where c.user_id == userId
                              select c).ToList();

                List<Object> myModel = new List<object>();
                myModel.Add(usermm);
                myModel.Add(myPost);
                myModel.Add(files);


                return View(myModel);
            }

            else
            {
                return View("Index");
            }
        }


        [HttpGet]
        public ActionResult CMSBeforeLogin()
        {
            var postList = (from p in database.Posts where p.activity_log.Equals("Accepted") orderby p.post_id ascending select p).ToList();


            string[] filePaths = Directory.GetFiles(Server.MapPath("~/Resources/Advetrisement_Image/"));
            List<AdvertisementDetail> files = new List<AdvertisementDetail>();
            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileName(filePath);
                files.Add(new AdvertisementDetail
                {

                    title = fileName.Split('.')[0].ToString(),
                    adImage = "../Resources/Advetrisement_Image/" + fileName
                });
            }

            List<Object> myModel = new List<object>();
            myModel.Add(postList);
            myModel.Add(files);

            return View(myModel);

        }

        [HttpGet]
        public ActionResult CMSAfterLogin()
        {
            //if (Session["LogedUserID"] != null)
            //{
            //To display current username
            //user user = new user();
            //int loginId = int.Parse(Session["LogedUserID"].ToString());
            //user = database.users.Find(loginId);


            //To display the posts
            var postList = (from p in database.Posts where p.activity_log.Equals("Accepted") orderby p.post_id ascending select p).ToList();


            string[] filePaths = Directory.GetFiles(Server.MapPath("~/Resources/Advetrisement_Image/"));
            List<AdvertisementDetail> files = new List<AdvertisementDetail>();
            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileName(filePath);
                files.Add(new AdvertisementDetail
                {

                    title = fileName.Split('.')[0].ToString(),
                    adImage = "../Resources/Advetrisement_Image/" + fileName
                });
            }


            //return View(files);
            ////To display the comments for posts
            //var commentlist = (from c in database.Comments select c).ToList();

            List<Object> myModel = new List<object>();
            myModel.Add(postList);
            myModel.Add(files);

            return View(myModel);
            //}

            //return View();
        }

    }
}