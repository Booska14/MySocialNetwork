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
        private MyContext context;
        private User currentUser;

        public CommentController()
        {
            context = new MyContext();
            currentUser = context.Users.Find(WebSecurity.CurrentUserId);
        }

        public ActionResult Index(int statusId)
        {
            var status = context.Status.Find(statusId);
            var comments = status.Comments.OrderBy(c => c.DateTime);

            foreach (var comment in comments)
            {
                comment.IsDeletable =
                    currentUser == comment.Author || currentUser == status.Author;
            }

            var viewModel = new CommentViewModel
            {
                Status = status,
                Comments = comments,
                CurrentUser = currentUser
            };

            return PartialView(viewModel);
        }

        [HttpPost]
        public ActionResult Add(int statusId, string message)
        {
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

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var comment = context.Comments.Find(id);

            context.Comments.Remove(comment);
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