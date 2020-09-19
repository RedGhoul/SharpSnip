using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Snips.Data
{
    public class ToDoList : AuditableEntity
    {
        public ToDoList()
        {
            this.Deleted = false;
        }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        [NotMapped]
        public NpgsqlTsVector SearchVector { get; set; }
        public ICollection<ToDoListItem> ToDoListItems { get; set; }
        public bool Deleted { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string? ApplicationUserId { get; set; }
    }
}
