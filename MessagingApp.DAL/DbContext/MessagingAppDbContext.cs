using MessagingApp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MessagingApp.DAL.DbContext
{
    public class MessagingAppDbContext:IdentityDbContext<User>
    {
        public MessagingAppDbContext()
        {
            
        }

        public MessagingAppDbContext(DbContextOptions<MessagingAppDbContext> options): base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //User can send multiple messages
            builder.Entity<Message>()
           .HasOne(m => m.Sender)
           .WithMany()
           .HasForeignKey(m => m.SenderId)
           .OnDelete(DeleteBehavior.Restrict);

            //User can receive multiple messages
            builder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=DESKTOP-FJ8OJV2; Database=MessagingAppDB; Uid=sa; Pwd=123;TrustServerCertificate=true;");
            }
        }

        public DbSet<Message> Messages { get; set; }
    }
}
