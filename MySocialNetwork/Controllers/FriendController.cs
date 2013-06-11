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
        private MyContext context = new MyContext();

        public ActionResult Index()
        {
            var requests = GetRequests();
            var users = GetUsers();

            var viewModel = new FriendViewModel
            {
                Requests = requests,
                Users = users
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AcceptRequest(Request request)
        {
            return null;
        }

        [HttpPost]
        public ActionResult DeclineRequest(Request request)
        {
            return null;
        }

        public ActionResult Search(string userName)
        {
            return null;
        }

        [HttpPost]
        public ActionResult SendRequest(User user)
        {
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }

        #region Helpers
        private IQueryable<Request> GetRequests()
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);
            var requests = context.Requests
                .Where(r => r.Receiver.Id == currentUser.Id
                    && r.Status == RequestStatus.Pending);

            return requests;
        }

        private IQueryable<User> GetUsers()
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);
            var users = context.Users
                .Where(u => u.Id != currentUser.Id);

            return users;
        }
        #endregion
    }
}
