using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccessControlManagement.Models
{
    public class PostAccess
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PostAccess()
        {
            this.Comments = new HashSet<Comment>();
        }
        public int post_id { get; set; }
        public System.DateTime post_date { get; set; }
        public int user_id { get; set; }
        public Nullable<int> category_id { get; set; }
        public string post_description { get; set; }
        public string activity_log { get; set; }
        public string title { get; set; }
        public string image { get; set; }

        public virtual Advertisement Advertisement { get; set; }
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual user user { get; set; }

    }
}