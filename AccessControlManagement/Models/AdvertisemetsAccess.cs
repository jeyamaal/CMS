using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccessControlManagement.Models
{
    public class AdvertisemetsAccess
    {
        public int ADD_id { get; set; }
        public string title { get; set; }
        public string category { get; set; }
        public string description { get; set; }
        public Nullable<System.DateTime> wantToPostDate { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> postedDate { get; set; }
        public Nullable<System.DateTime> updatedDate { get; set; }
        public Nullable<System.DateTime> dueDate { get; set; }

        public virtual Category Category1 { get; set; }
    }
}

