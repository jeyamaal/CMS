using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AccessControlManagement.Models
{
    public class CategoryUsers : ApiController
    {
        public string firstName { set; get; }
        public string userName { set; get; }
        public string email { set; get; }
        public int postCount { set; get; }
    }
}