using MySocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySocialNetwork.ViewModels
{
    public class CommentViewModel
    {
        public Comment Comment { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
}