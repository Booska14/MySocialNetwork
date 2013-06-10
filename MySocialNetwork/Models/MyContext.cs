using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Friends)
                .WithMany();

            modelBuilder.Entity<Status>()
                .Ignore(s => s.IsDeletable);

            modelBuilder.Entity<Comment>()
                .HasRequired(c => c.Status)
                .WithMany(s => s.Comments)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Comment>()
                .Ignore(c => c.IsDeletable);

            modelBuilder.Entity<Comment>()
                .Ignore(c => c.IsUpdatable);
        }
    }
}