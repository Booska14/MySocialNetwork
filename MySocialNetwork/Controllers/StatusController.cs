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
            var statuses = GetStatuses();

            var viewModel = new StatusViewModel
            {
                Status = new Status(),
                Statuses = statuses
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Create(Status model)
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);

            var status = new Status
            {
                Author = currentUser,
                DateTime = DateTime.Now,
                Message = model.Message,
                IsDeletable = true
            };

            context.Status.Add(status);
            context.SaveChanges();

            return PartialView("DetailsPartial", status);
        }

        [HttpPost]
        public ActionResult Delete(Status model)
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);
            var status = context.Status.Find(model.Id);

            context.Status.Remove(status);
            context.SaveChanges();

            var statuses = GetStatuses();

            ModelState.Clear();

            return PartialView("ListPartial", statuses);
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }

        #region Helpers
        private IQueryable<Status> GetStatuses()
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

            return statuses;
        }
        #endregion
    }
}
