using MySocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySocialNetwork.ViewModels
{
    public class CommentViewModel
    {
        public Status Status { get; set; }
        public string Message { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}