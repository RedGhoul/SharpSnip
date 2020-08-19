using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Snips.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ToDoList> ToDoLists { get; set; }
        public DbSet<ToDoListItem> ToDoListItems { get; set; }
        public DbSet<Note> Notes { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = DateTime.UtcNow;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Note>()
            .HasOne<ApplicationUser>(note => note.ApplicationUser)
            .WithMany(appuser => appuser.Notes)
            .HasForeignKey(note => note.ApplicationUserId);

            builder.Entity<ToDoList>()
           .HasOne<ApplicationUser>(todolist => todolist.ApplicationUser)
           .WithMany(appuser => appuser.ToDoLists)
           .HasForeignKey(todolist => todolist.ApplicationUserId);

            builder.Entity<ToDoListItem>()
           .HasOne<ToDoList>(ToDoListItem => ToDoListItem.ToDoList)
           .WithMany(todolist => todolist.ToDoListItems)
           .HasForeignKey(todolistitem => todolistitem.ToDoListId);

            builder.Entity<Note>().HasIndex(n => n.LastModified);
            builder.Entity<Note>().HasIndex(n => n.Created);
            builder.Entity<Note>().HasIndex(n => n.SearchVector)
                .HasMethod("GIN");

            builder.Entity<ToDoListItem>().HasIndex(n => n.LastModified);
            builder.Entity<ToDoListItem>().HasIndex(n => n.Created);
            builder.Entity<ToDoListItem>().HasIndex(n => n.SearchVector)
                .HasMethod("GIN");

            builder.Entity<ToDoList>().HasIndex(n => n.LastModified);
            builder.Entity<ToDoList>().HasIndex(n => n.Created);
            builder.Entity<ToDoList>().HasIndex(n => n.SearchVector)
                .HasMethod("GIN");


        }
    }
}
