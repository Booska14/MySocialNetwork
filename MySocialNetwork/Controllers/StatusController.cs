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
            var status = context.Status
                .Where(s => s.User.Id == currentUser.Id
                    || friendIds.Any(f => f == s.User.Id))
                .OrderByDescending(s => s.DateTime);

            var viewModel = new StatusIndexViewModel
            {
                Status = status
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Update(string statusMessage)
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);

            var status = new Status
            {
                User = currentUser,
                Message = statusMessage,
                DateTime = DateTime.Now
            };

            context.Status.Add(status);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Comment(int statusId, string message)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Like(int statusId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Dislike(int statusId)
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }
    }
}
