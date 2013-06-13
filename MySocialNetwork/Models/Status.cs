using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySocialNetwork.Models
{
    public class Status
    {
        public Status()
        {
            if (Comments == null)
                Comments = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public virtual string Text { get; set; }
        public virtual DateTime DateTime { get; set; }

        public virtual User Author { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public bool IsDeletable { get; set; }
    }
}