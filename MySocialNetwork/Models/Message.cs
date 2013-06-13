using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySocialNetwork.Models
{
    public class Message
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Text { get; set; }

        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
    }
}