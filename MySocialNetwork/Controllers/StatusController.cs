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
            var status = context.Status.ToList();

            var viewModel = new StatusIndexViewModel
            {
                Status = status
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(string statusMessage)
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

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }
    }
}
