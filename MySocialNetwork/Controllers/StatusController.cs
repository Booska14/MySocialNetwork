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
        private User currentUser;

        public StatusController()
        {
            context = new MyContext();
            currentUser = context.Users.Find(WebSecurity.CurrentUserId);
        }

        public ActionResult Index()
        {
            var friendIds = currentUser.Friends.Select(f => f.Id);
            var status = context.Status
                .Where(s => s.Author.Id == currentUser.Id
                    || friendIds.Any(f => f == s.Author.Id))
                .OrderByDescending(s => s.DateTime);

            foreach (var s in status)
            {
                s.IsDeletable = currentUser == s.Author;
            }

            var viewModel = new StatusViewModel
            {
                CurrentUser = currentUser,
                Status = status
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Add(string message)
        {
            var status = new Status
            {
                Author = currentUser,
                Message = message,
                DateTime = DateTime.Now
            };

            context.Status.Add(status);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var status = context.Status.Find(id);

            context.Status.Remove(status);
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
