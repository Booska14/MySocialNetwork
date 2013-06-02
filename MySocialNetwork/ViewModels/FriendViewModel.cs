using MySocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySocialNetwork.ViewModels
{
    public class FriendViewModel
    {
        public IEnumerable<User> Friends { get; set; }
        public string Name { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}