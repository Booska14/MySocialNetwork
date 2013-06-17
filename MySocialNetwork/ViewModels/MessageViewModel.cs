using MySocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySocialNetwork.ViewModels
{
    public class MessageViewModel
    {
        public IEnumerable<User> Friends { get; set; }
        public User Friend { get; set; }
        public IEnumerable<Message> Messages { get; set; }
    }
}