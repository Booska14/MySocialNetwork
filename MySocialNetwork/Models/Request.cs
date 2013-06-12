using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySocialNetwork.Models
{
    public class Request
    {
        public int Id { get; set; }
        public virtual DateTime DateTime { get; set; }
        public virtual RequestStatus Status { get; set; }
        public virtual DateTime StatusDateTime { get; set; }

        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
    }

    public enum RequestStatus
    {
        Pending,
        Accepted,
        Declined
    }
}