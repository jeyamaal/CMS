using AccessControlManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Advertisement/Create
        public ActionResult Create(InsertAdd s)
        {
            try
            {
                s.status = "Pending";
                db.InsertAdds.Add(s);
                db.SaveChanges();
                ModelState.Clear();

                ViewBag.Message = "Successfully add your addvertisment";

            }

            catch (Exception e)
            {

            }
            return View();

        }

        // POST: Advertisement/Create
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

        // GET: Advertisement/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Advertisement/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Advertisement/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Advertisement/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
