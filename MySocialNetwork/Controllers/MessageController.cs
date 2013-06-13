using MySocialNetwork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MySocialNetwork.Controllers
{
    public class MessageController : Controller, IDisposable
    {
        private MyContext context;

        public MessageController()
        {
            context = new MyContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }
    }
}
