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
    public class HomeController : Controller, IDisposable
    {
        private MyContext context;

        public HomeController()
        {
            context = new MyContext();
        }

        public ActionResult Index()
        {
            var viewModel = new HomeIndexViewModel
            {
                Status = context.Status.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(Status model)
        {
            model.DateTime = DateTime.Now;
            model.User = context.Users.Find(WebSecurity.CurrentUserId);

            context.Status.Add(model);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (context != null)
            {
                context.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
