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
        public virtual DbSet<Chapter> Chapter { get; set; }
        public virtual DbSet<Follow> Follow { get; set; }
        public virtual DbSet<ReadHistory> ReadHistory { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<User_Login> User_Login { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Books>()
                .HasMany(e => e.ReadHistory)
                .WithRequired(e => e.Books)
                .HasForeignKey(e => e.BookId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Categories>()
                .HasMany(e => e.Books)
                .WithRequired(e => e.Categories)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Chapter>()
                .HasMany(e => e.ReadHistory)
                .WithRequired(e => e.Chapter)
                .HasForeignKey(e => e.ChapId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Status)
                .IsFixedLength();

            modelBuilder.Entity<User>()
                .HasMany(e => e.ReadHistory)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.UserName)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.User_Login)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.UserName)
                .WillCascadeOnDelete(false);
        }
    }
}
