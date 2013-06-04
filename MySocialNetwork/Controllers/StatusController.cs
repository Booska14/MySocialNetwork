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
        private MyContext context = new MyContext();

        public ActionResult Index()
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);
            var friendIds = currentUser.Friends.Select(f => f.Id);
            var status = context.Status
                .Where(s => s.Author.Id == currentUser.Id
                    || friendIds.Any(f => f == s.Author.Id))
                .OrderByDescending(s => s.DateTime);

            return View(status);
        }

        [HttpPost]
        public ActionResult Add(string message)
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);

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
        public ActionResult Like(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Dislike(int id)
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
