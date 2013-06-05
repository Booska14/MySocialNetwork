using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySocialNetwork.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public virtual User Author { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
        public bool IsDeletable { get; set; }
        public virtual Status Status { get; set; }
    }
}