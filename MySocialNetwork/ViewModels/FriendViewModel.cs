using MySocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySocialNetwork.ViewModels
{
    public class FriendViewModel
    {
        public IEnumerable<Request> Requests { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}