using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MySocialNetwork.Models
{
    public class User
    {
        public int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

        public virtual ICollection<User> Friends { get; set; }
        public virtual ICollection<Request> SentRequests { get; set; }
        public virtual ICollection<Request> ReceivedRequests { get; set; }
        public virtual ICollection<Message> SentMessages { get; set; }
        public virtual ICollection<Message> ReceivedMessages { get; set; }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public bool CanCreate(Comment comment)
        {
            return Id == comment.Status.Author.Id || Friends.Any(f => f.Id == comment.Status.Author.Id);
        }

        public bool CanUpdate(Comment comment)
        {
            return Id == comment.Author.Id;
        }

        public bool CanDelete(Status status)
        {
            return Id == status.Author.Id;
        }

        public bool CanDelete(Comment comment)
        {
            return Id == comment.Author.Id || Id == comment.Status.Author.Id;
        }
    }
}