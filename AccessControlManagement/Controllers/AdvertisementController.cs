using AccessControlManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Diagnostics;




namespace AccessControlManagement.Controllers
{
    public class AdvertisementController : Controller
    {
        CMSEntities db = new CMSEntities();
        // GET: Advertisement
        //User can view the history 
        public ActionResult Index()
        {
            var advertisementDetails = db.AdvertisementDetails.Include(b=>b.Category1);
            return View(advertisementDetails.ToList());
        }

        // GET: Advertisement/Details/5
        //user can view the each advertisemnet 
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdvertisementDetail advertisementDetail = db.AdvertisementDetails.Find(id);
            if (advertisementDetail == null)
            {
                return HttpNotFound();
            }
            return View(advertisementDetail);
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
        // GET: Advertisement/Create
        public ActionResult Create()
        {



            ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name");
            return View();
        }

        // POST: Advertisement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
       
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file, AdvertisementDetail advertisementDetail)
        {

            string db_path = null;

            if (ModelState.IsValid && file.ContentLength > 0 && file.ContentType.Contains("image"))
            {
                var fileName = Path.GetFileName(file.FileName);
                var files = Path.GetExtension(".jpg");

                if (files != null)
                {

                    var img = Image.FromStream(file.InputStream);


                    if (img.RawFormat.Equals(ImageFormat.Png) || img.RawFormat.Equals(ImageFormat.Jpeg))
                    {

                        string filestring = fileName.ToString();
                        string dir = "~/Resources/Advertisement_Image";
                        var path = Path.Combine(Server.MapPath(dir), fileName);
                        try
                        {
                            if (!Directory.Exists(dir))
                            {
                                Directory.CreateDirectory(dir);
                            }
                            else {

                            }
                            file.SaveAs(path);
                            string absoulte_path = path.ToString();
                            db_path = "\\" + GetRightPartOfPath(absoulte_path, "Resources") + "\\" + filestring;

                        }
                        catch
                        {
                            Debug.WriteLine("Error");
                        }


                    }

                    else
                    {
                        //TempData["Message1"] = "Profile Updated Successfully";
                        return JavaScript("<script>alert(\"Invalidformat\")</script>");

                    }




                }
                Guid guid = Guid.NewGuid();
                Random random = new Random();
                int i = random.Next();
              
                advertisementDetail.ADD_id = i;
                advertisementDetail.status = "Pending";
                advertisementDetail.adImage= db_path;
                db.AdvertisementDetails.Add(advertisementDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name", advertisementDetail.category_id);
            return View(advertisementDetail);
        }

        // GET: Advertisement/Edit/5
        // User can update the advertisemnet details 
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdvertisementDetail advertisementDetail = db.AdvertisementDetails.Find(id);
            if (advertisementDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name", advertisementDetail.category_id);
            return View(advertisementDetail);
        }

        // POST: Advertisement/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ADD_id,title,category_id,description,wantToPostDate,status,postedDate,updatedDate,dueDate")] AdvertisementDetail advertisementDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(advertisementDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name", advertisementDetail.category_id);
            return View(advertisementDetail);
        }

        //User can delete the advertisement details.
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AdvertisementDetail advertisementDetail = db.AdvertisementDetails.Find(id);
            if (advertisementDetail == null)
            {
                return HttpNotFound();
            }
            return View(advertisementDetail);
        }

        // POST: Advertisement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AdvertisementDetail advertisementDetail = db.AdvertisementDetails.Find(id);
            db.AdvertisementDetails.Remove(advertisementDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
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
