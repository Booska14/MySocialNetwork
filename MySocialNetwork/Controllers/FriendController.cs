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
            var friends = currentUser.Friends;
            var friendIds = friends.Select(f => f.Id);
            var users = context.Users
                .Where(u => u.Id != currentUser.Id
                    && !friendIds.Any(f => f == u.Id));

            var viewModel = new FriendIndexViewModel
            {
                Friends = friends,
                Users = users
            };

            return View(viewModel);
        }

        public ActionResult Search(string userName)
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);
            var friends = currentUser.Friends.OrderBy(f => f.Name);
            var friendIds = friends.Select(f => f.Id);
            var users = context.Users
                .Where(u => u.Name.Contains(userName)
                    && u.Id != currentUser.Id
                    && !friendIds.Any(f => f == u.Id))
                .OrderBy(u => u.Name);

            return PartialView("UsersPartial", users);
        }

        [HttpPost]
        public ActionResult Add(User user)
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);
            var friend = context.Users.Find(user.Id);

            currentUser.Friends.Add(friend);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Remove(User user)
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);
            var friend = context.Users.Find(user.Id);

            currentUser.Friends.Remove(friend);
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
