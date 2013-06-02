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
    public class UserController : Controller, IDisposable
    {
        private MyContext context = new MyContext();

        public ActionResult Index()
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);
            var friends = currentUser.Friends;
            var friendIds = friends.Select(f => f.Id);
            var users = context.Users
                .Where(u => u.Id != currentUser.Id
                    && !friendIds.Any(f => f == u.Id));

            var viewModel = new FriendViewModel
            {
                Friends = friends,
                Users = users
            };

            return View(viewModel);
        }

        public ActionResult Search(string name)
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);
            var friends = currentUser.Friends.OrderBy(f => f.Name);
            var friendIds = friends.Select(f => f.Id);
            var users = context.Users
                .Where(u => u.Name.Contains(name)
                    && u.Id != currentUser.Id
                    && !friendIds.Any(f => f == u.Id))
                .OrderBy(u => u.Name);

            return PartialView("UsersPartial", users);
        }

        [HttpPost]
        public ActionResult AddToFriends(int userId)
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);
            var friend = context.Users.Find(userId);

            currentUser.Friends.Add(friend);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveFromFriends(int userId)
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);
            var friend = context.Users.Find(userId);

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
