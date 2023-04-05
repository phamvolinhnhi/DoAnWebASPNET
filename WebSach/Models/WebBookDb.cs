using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebSach.Models
{
    public partial class WebBookDb : DbContext
    {
        public WebBookDb()
            : base("name=WebBookDb")
        {
        }

        public virtual DbSet<Books> Books { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<Chapter> Chapter { get; set; }
        public virtual DbSet<Follow> Follow { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Books>()
                .HasMany(e => e.Chapter)
                .WithRequired(e => e.Books)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Categories>()
                .HasMany(e => e.Books)
                .WithRequired(e => e.Categories)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Status)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .HasMany(e => e.Books)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Comment)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);
        }
    }
}
