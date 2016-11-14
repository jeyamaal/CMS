using AccessControlManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

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

        // GET: Advertisement/Create
        public ActionResult Create()
        {



            ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name");
            return View();
        }

        // POST: Advertisement/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ADD_id,title,category_id,description,wantToPostDate,status,postedDate,updatedDate,dueDate")] AdvertisementDetail advertisementDetail)
        {
            if (ModelState.IsValid)
            {

                Guid guid = Guid.NewGuid();
                Random random = new Random();
                int i = random.Next();

                advertisementDetail.ADD_id = i;
                advertisementDetail.status = "Pending";
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
