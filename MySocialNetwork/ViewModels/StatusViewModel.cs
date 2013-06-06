using MySocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySocialNetwork.ViewModels
{
    public class StatusViewModel
    {
        public Status Status { get; set; }
        public IEnumerable<Status> Statuses { get; set; }
    }
}