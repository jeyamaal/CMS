using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AccessControlManagement.Models;
using System.IO;

namespace AccessControlManagement.Controllers
{
    public class ViewAdvertisement : Controller
    {
        private CMSEntities db = new CMSEntities();

        public ActionResult Index()
        {
            string[] filePaths = Directory.GetFiles(Server.MapPath("~/assets/img/"));
            List<AdvertisementDetail> files = new List<AdvertisementDetail>();
            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileName(filePath);
                files.Add(new AdvertisementDetail
                {
                    title = fileName.Split('.')[0].ToString(),
                    adImage = "../Resources/Advetrisement_Image" + fileName
                });
            }

            return View(files);
        }
    }
}
