using MySocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySocialNetwork.ViewModels
{
    public class StatusViewModel
    {
        public IEnumerable<Status> Status { get; set; }
        public User CurrentUser { get; set; }
    }
}