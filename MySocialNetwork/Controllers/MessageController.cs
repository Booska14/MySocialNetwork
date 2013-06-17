using MySocialNetwork.Models;
using MySocialNetwork.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

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
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);
            var friends = currentUser.Friends
                .OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName);
            var friend = friends.First();
            var sentMessages = friend.SentMessages;
            var receivedMessages = friend.ReceivedMessages;
            var messages = sentMessages.Union(receivedMessages)
                .OrderBy(m => m.DateTime);

            var viewModel = new MessageViewModel
            {
                Friends = friends,
                Friend = friend,
                Messages = messages
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SelectFriend(User model)
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);
            var friend = context.Users.Find(model.Id);
            var sentMessages = currentUser.SentMessages
                .Where(m => m.Receiver.Id == friend.Id);
            var receivedMessages = currentUser.ReceivedMessages
                .Where(m => m.Sender.Id == friend.Id);
            var messages = sentMessages.Union(receivedMessages)
                .OrderBy(m => m.DateTime);

            return PartialView("MessagesPartial", messages);
        }

        [HttpPost]
        public ActionResult SendMessage(User friend, string text)
        {
            var currentUser = context.Users.Find(WebSecurity.CurrentUserId);
            var user = context.Users.Find(friend.Id);

            var message = new Message
            {
                Sender = currentUser,
                Receiver = user,
                Text = text,
                DateTime = DateTime.Now
            };

            context.Messages.Add(message);
            context.SaveChanges();

            return PartialView("MessagePartial", message);
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }
    }
}
