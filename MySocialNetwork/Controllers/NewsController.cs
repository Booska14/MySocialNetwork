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
        private User currentUser;

        public NewsController()
        {
            context = new MyContext();
            currentUser = context.Users.Find(WebSecurity.CurrentUserId);
        }

        public ActionResult Index()
        {
            var statuses = Statuses();

            return View(statuses);
        }

        [HttpPost]
        public ActionResult AddStatus(string text)
        {
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
            var status = context.Status.Find(model.Id);

            if (currentUser.CanDelete(status))
            {
                status.Comments.ToList().ForEach(c => context.Comments.Remove(c));

                context.Status.Remove(status);
                context.SaveChanges();
            }

            var statuses = Statuses();

            ModelState.Clear();
            return PartialView("StatusesPartial", statuses);
        }

        [HttpPost]
        public ActionResult AddComment(int id, string text)
        {
            var status = context.Status.Find(id);
            Comment comment = new Comment
            {
                Status = status,
                Author = currentUser,
                DateTime = DateTime.Now,
                Text = text,
                IsDeletable = true,
                IsUpdatable = true
            };

            if (currentUser.CanCreate(comment))
            {
                context.Comments.Add(comment);
                context.SaveChanges();
            }

            ModelState.Clear();
            return PartialView("CommentPartial", comment);
        }

        [HttpPost]
        public ActionResult UpdateComment(Comment model)
        {
            var comment = context.Comments.Find(model.Id);

            if (currentUser.CanUpdate(comment))
            {
                comment.Text = model.Text;

                context.SaveChanges();
            }

            var comments = CommentsByStatus(comment.Status);

            ModelState.Clear();
            return PartialView("CommentsPartial", comments);
        }

        [HttpPost]
        public ActionResult RemoveComment(Comment model)
        {
            var comment = context.Comments.Find(model.Id);
            var status = context.Status.Find(comment.Status.Id);

            if (currentUser.CanDelete(comment))
            {
                context.Comments.Remove(comment);
                context.SaveChanges();
            }

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
                status.IsDeletable = currentUser.CanDelete(status);

                foreach (var comment in status.Comments)
                {
                    comment.IsUpdatable = currentUser.CanUpdate(comment);
                    comment.IsDeletable = currentUser.CanDelete(comment);
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
                comment.IsUpdatable = currentUser.CanUpdate(comment);
                comment.IsDeletable = currentUser.CanDelete(comment);
            }

            return comments;
        }
        #endregion
    }
}
