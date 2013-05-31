using MySocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySocialNetwork.ViewModels
{
    public class StatusIndexViewModel
    {
        public string StatusMessage { get; set; }
        public IEnumerable<Status> Status { get; set; }
    }
}