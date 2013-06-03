using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MySocialNetwork.Models;
using MySocialNetwork.ViewModels;
using WebMatrix.WebData;

namespace MySocialNetwork.Controllers
{
    public class CommentController : Controller
    {
        private MyContext context = new MyContext();

        public ActionResult Index(int statusId)
        {
            var status = context.Status.Find(statusId);
            var comments = status.Comments.OrderBy(c => c.DateTime);

            var viewModel = new CommentViewModel
            {
                Status = status,
                Comments = comments
            };

            return PartialView(viewModel);
        }

        [HttpPost]
        public ActionResult Add(int statusId, string message)
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);
            var status = context.Status.Find(statusId);

            var comment = new Comment
            {
                Author = currentUser,
                Message = message,
                DateTime = DateTime.Now,
                Status = status
            };

            context.Comments.Add(comment);
            context.SaveChanges();

            return RedirectToAction("Index", "Status");
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }
    }
}