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

        public ActionResult Index(Status status)
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);

            foreach (var comment in status.Comments)
            {
                comment.IsDeletable = currentUser.Id == comment.Author.Id || currentUser.Id == status.Author.Id;
                comment.IsUpdatable = currentUser.Id == comment.Author.Id;
            }

            var viewModel = new CommentViewModel
            {
                Comment = new Comment {
                    Status = status
                },
                Comments = status.Comments
            };

            return PartialView(viewModel);
        }

        [HttpPost]
        public ActionResult Create(Comment comment)
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);
            var status = context.Status.Find(comment.Status.Id);

            var commentToCreate = new Comment
            {
                Author = currentUser,
                Message = comment.Message,
                DateTime = DateTime.Now,
                Status = status
            };

            context.Comments.Add(commentToCreate);
            context.SaveChanges();

            return RedirectToAction("Index", "Status");
        }

        [HttpPost]
        public ActionResult Delete(Comment comment)
        {
            var commentToRemove = context.Comments.Find(comment.Id);

            context.Comments.Remove(commentToRemove);
            context.SaveChanges();

            return RedirectToAction("Index", "Status");
        }

        [HttpPost]
        public ActionResult Update(Comment comment)
        {
            var commentToUpdate = context.Comments.Find(comment.Id);

            commentToUpdate.Message = comment.Message;
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