using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snips.Data
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.DateCreated = DateTime.UtcNow;
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Note> Notes { get; set; }
        public ICollection<ToDoList> ToDoLists { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
