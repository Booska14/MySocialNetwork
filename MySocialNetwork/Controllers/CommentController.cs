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

        public CommentController()
        {
            context = new MyContext();
        }

        public ActionResult Index(Status model)
        {
            var status = context.Status.Find(model.Id);
            var comments = GetComments(status);

            var viewModel = new CommentViewModel
            {
                Comment = new Comment
                {
                    Status = status
                },
                Comments = status.Comments
            };

            ModelState.Clear();

            return PartialView(viewModel);
        }

        [HttpPost]
        public ActionResult Create(Comment model)
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);
            var status = context.Status.Find(model.Status.Id);

            var comment = new Comment
            {
                Status = status,
                Author = currentUser,
                DateTime = DateTime.Now,
                Message = model.Message,
                IsDeletable = true,
                IsUpdatable = true
            };

            context.Comments.Add(comment);
            context.SaveChanges();

            return PartialView("DetailsPartial", comment);
        }

        [HttpPost]
        public ActionResult Delete(Comment model)
        {
            var comment = context.Comments.Find(model.Id);
            var status = context.Status.Find(comment.Status.Id);

            context.Comments.Remove(comment);
            context.SaveChanges();

            var comments = GetComments(status);

            ModelState.Clear();

            return PartialView("ListPartial", comments);
        }

        [HttpPost]
        public ActionResult Update(Comment model)
        {
            var comment = context.Comments.Find(model.Id);

            comment.Message = model.Message;
            context.SaveChanges();

            var status = context.Status.Find(comment.Status.Id);
            var comments = GetComments(status);

            ModelState.Clear();

            return PartialView("ListPartial", comments);
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }

        #region Helpers
        private IQueryable<Comment> GetComments(Status status)
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);
            var comments = context.Comments
                .Where(c => c.Status.Id == status.Id);

            foreach (var comment in comments)
            {
                comment.IsDeletable = currentUser.Id == comment.Author.Id || currentUser.Id == status.Author.Id;
                comment.IsUpdatable = currentUser.Id == comment.Author.Id;
            }

            return comments;
        }
        #endregion
    }
}