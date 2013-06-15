using MySocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace MySocialNetwork.Controllers
{
    [Authorize]
    public class NewsController : Controller, IDisposable
    {
        private MyContext context;

        public NewsController()
        {
            context = new MyContext();
        }

        public ActionResult Index()
        {
            var statuses = Statuses();

            return View(statuses);
        }

        [HttpPost]
        public ActionResult AddStatus(string text)
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);

            var status = new Status
            {
                Author = currentUser,
                DateTime = DateTime.Now,
                Text = text,
                IsDeletable = true
            };

            context.Status.Add(status);
            context.SaveChanges();

            ModelState.Clear();
            return PartialView("StatusPartial", status);
        }

        [HttpPost]
        public ActionResult RemoveStatus(Status model)
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);
            var status = context.Status.Find(model.Id);

            //Could be done with delete on cascade
            status.Comments.ToList().ForEach(c => context.Comments.Remove(c));

            context.Status.Remove(status);
            context.SaveChanges();

            var statuses = Statuses();

            ModelState.Clear();
            return PartialView("StatusesPartial", statuses);
        }

        [HttpPost]
        public ActionResult AddComment(int id, string text)
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);
            var status = context.Status.Find(id);

            var comment = new Comment
            {
                Status = status,
                Author = currentUser,
                DateTime = DateTime.Now,
                Text = text,
                IsDeletable = true,
                IsUpdatable = true
            };

            context.Comments.Add(comment);
            context.SaveChanges();

            ModelState.Clear();
            return PartialView("CommentPartial", comment);
        }

        [HttpPost]
        public ActionResult UpdateComment(Comment model)
        {
            var comment = context.Comments.Find(model.Id);
            comment.Text = model.Text;

            context.SaveChanges();

            var comments = CommentsByStatus(comment.Status);

            ModelState.Clear();
            return PartialView("CommentsPartial", comments);
        }

        [HttpPost]
        public ActionResult RemoveComment(Comment model)
        {
            var comment = context.Comments.Find(model.Id);
            var status = context.Status.Find(comment.Status.Id);

            context.Comments.Remove(comment);
            context.SaveChanges();

            var comments = CommentsByStatus(status);

            ModelState.Clear();
            return PartialView("CommentsPartial", comments);
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }

        #region Helpers
        private IQueryable<Status> Statuses()
        {
            var currentUser = context.Users
                .Find(WebSecurity.CurrentUserId);

            var friends = currentUser.Friends
                .Select(f => f.Id);

            var statuses = context.Status
                .Where(s => s.Author.Id == currentUser.Id
                    || friends.Any(f => f == s.Author.Id))
                .OrderByDescending(s => s.DateTime);

            foreach (var status in statuses)
            {
                status.IsDeletable = currentUser == status.Author;

                foreach (var comment in status.Comments)
                {
                    comment.IsDeletable = currentUser.Id == comment.Author.Id || currentUser.Id == status.Author.Id;
                    comment.IsUpdatable = currentUser.Id == comment.Author.Id;
                }
            }

            return statuses;
        }

        private IQueryable<Comment> CommentsByStatus(Status status)
        {
            var currentUser = context.Users
                .Find(WebSecurity.CurrentUserId);

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
