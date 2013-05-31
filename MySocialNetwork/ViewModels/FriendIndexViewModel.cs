using MySocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySocialNetwork.ViewModels
{
    public class FriendIndexViewModel
    {
        public IEnumerable<User> Friends { get; set; }
        public string UserName { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}