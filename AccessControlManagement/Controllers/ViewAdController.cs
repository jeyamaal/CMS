using AccessControlManagement.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccessControlManagement.Controllers
{
    public class ViewAdController : Controller
    {
        // GET: ViewAd
        public ActionResult Index()
        {
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


            return View(files);
        }
    }
}