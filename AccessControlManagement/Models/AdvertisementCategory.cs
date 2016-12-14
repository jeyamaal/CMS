using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccessControlManagement.Models
{
    public class AdvertisementCategory
    {
        public int adID { set; get; }
        public string categoryName { set; get; }
        public string postedDate { set; get; }
        public string title { set; get; }
        public string status { set; get; }
        public string expirayDate { set; get; }
    }
}