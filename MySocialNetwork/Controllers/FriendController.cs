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
    public class FriendController : Controller, IDisposable
    {
        private MyContext context;

        public FriendController()
        {
            context = new MyContext();
        }

        public ActionResult Index()
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);
            var friends = currentUser.Friends.ToList();
            var users = context.Users.ToList().Except(friends).Where(u => u != currentUser);

            var viewModel = new FriendIndexViewModel
            {
                Friends = friends,
                Users = users
            };

            return View(viewModel);
        }

        public ActionResult Search(string userName)
        {
            var users = context.Users.Where(u => u.Name.Contains(userName));

            return PartialView("UsersPartial", users);
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }
    }
}
