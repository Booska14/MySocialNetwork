using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace MySocialNetwork.Models
{
    public class MyContext : DbContext
    {
        public MyContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new StatusConfiguration());
            modelBuilder.Configurations.Add(new CommentConfiguration());
        }
    }

    #region Configurations
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            HasMany(u => u.Friends)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("Friends");
                    m.MapRightKey("Friend_Id");
                });

            HasMany(u => u.SentRequests)
                .WithRequired(r => r.Sender)
                .WillCascadeOnDelete(false);

            HasMany(u => u.ReceivedRequests)
                .WithRequired(r => r.Receiver)
                .WillCascadeOnDelete(false);

            HasMany(u => u.SentMessages)
                .WithRequired(m => m.Sender)
                .WillCascadeOnDelete(false);

            HasMany(u => u.ReceivedMessages)
                .WithRequired(m => m.Receiver)
                .WillCascadeOnDelete(false);
        }
    }

    public class StatusConfiguration : EntityTypeConfiguration<Status>
    {
        public StatusConfiguration()
        {
            Ignore(s => s.IsDeletable);
        }
    }

    public class CommentConfiguration : EntityTypeConfiguration<Comment>
    {
        public CommentConfiguration()
        {
            //HasRequired(c => c.Status)
            //    .WithMany(s => s.Comments)
            //    .WillCascadeOnDelete(true);

            Ignore(c => c.IsDeletable);
            Ignore(c => c.IsUpdatable);
        }
    }
    #endregion
}