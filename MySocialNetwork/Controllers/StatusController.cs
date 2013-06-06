using MySocialNetwork.Models;
using MySocialNetwork.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace MySocialNetwork.Controllers
{
    [Authorize]
    public class StatusController : Controller, IDisposable
    {
        private MyContext context;

        public StatusController()
        {
            context = new MyContext();
        }

        public ActionResult Index()
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);
            var friendIds = currentUser.Friends.Select(f => f.Id);
            var statuses = context.Status
                .Where(s => s.Author.Id == currentUser.Id || friendIds.Any(f => f == s.Author.Id))
                .OrderByDescending(s => s.DateTime);

            foreach (var status in statuses)
            {
                status.IsDeletable = currentUser == status.Author;
            }

            var viewModel = new StatusViewModel
            {
                Status = new Status(),
                Statuses = statuses
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(Status status)
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);

            var statusToCreate = new Status
            {
                Author = currentUser,
                Message = status.Message,
                DateTime = DateTime.Now
            };

            context.Status.Add(statusToCreate);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(Status status)
        {
            var statusToDelete = context.Status.Find(status.Id);

            context.Status.Remove(statusToDelete);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }
    }
}
