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
            var requests = Requests();
            var users = Users();

            var viewModel = new FriendViewModel
            {
                Requests = requests,
                Users = users
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult AcceptRequest(Request model)
        {
            var request = context.Requests
                .Find(model.Id);

            var sender = request.Sender;
            var receiver = request.Receiver;

            request.Status = RequestStatus.Accepted;
            request.StatusDateTime = DateTime.Now;

            sender.Friends.Add(receiver);
            receiver.Friends.Add(sender);

            context.SaveChanges();

            var requests = Requests();

            return PartialView("RequestsPartial", requests);
        }

        [HttpPost]
        public ActionResult DeclineRequest(Request model)
        {
            var request = context.Requests
                .Find(model.Id);

            request.Status = RequestStatus.Declined;
            request.StatusDateTime = DateTime.Now;

            context.SaveChanges();

            var requests = Requests();

            return PartialView("RequestsPartial", requests);
        }

        public ActionResult Search(string name)
        {
            var currentUser = context.Users
                .Find(WebSecurity.CurrentUserId);

            var users = UsersByName(name);

            return PartialView("UsersPartial", users);
        }

        [HttpPost]
        public ActionResult SendRequest(User model)
        {
            var currentUser = context.Users
                .Find(WebSecurity.CurrentUserId);

            var user = context.Users
                .Find(model.Id);

            var request = new Request
            {
                Sender = currentUser,
                Receiver = user,
                DateTime = DateTime.Now,
                StatusDateTime = DateTime.Now
            };

            context.Requests.Add(request);
            context.SaveChanges();

            var users = Users();

            return PartialView("UsersPartial", users);
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }

        #region Helpers
        private IQueryable<Request> Requests()
        {
            var currentUser = context.Users
                .Find(WebSecurity.CurrentUserId);

            // Requests I have received and which are pending
            var requests = context.Requests
                .Where(r => r.Receiver.Id == currentUser.Id
                    && r.Status == RequestStatus.Pending);

            return requests;
        }

        private IQueryable<User> Users()
        {
            var currentUser = context.Users
                .Find(WebSecurity.CurrentUserId);

            var friends = currentUser.Friends
                .Select(f => f.Id);

            var receivers = context.Requests
                .Where(r => r.Sender.Id == currentUser.Id)
                .Select(r => r.Receiver.Id);

            var senders = context.Requests
                .Where(r => r.Receiver.Id == currentUser.Id)
                .Select(r => r.Sender.Id);

            // Users who are not me, not part of my friends,
            // who I haven't sent a request to and who haven't sent me a request
            var users = context.Users
                .Where(u => u.Id != currentUser.Id
                    && !friends.Any(f => f == u.Id)
                    && !receivers.Any(s => s == u.Id)
                    && !senders.Any(r => r == u.Id))
                .OrderBy(u => u.FullName);

            return users;
        }

        private IQueryable<User> UsersByName(string name)
        {
            var users = Users()
                .Where(u => u.FullName.Contains(name));

            return users;
        }
        #endregion
    }
}
