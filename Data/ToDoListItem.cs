using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Snips.Data
{
    public class ToDoListItem : AuditableEntity
    {
        public ToDoListItem()
        {
            this.Deleted = false;
        }
        public int Id { get; set; }
        public string Content { get; set; }
        [NotMapped]
        public NpgsqlTsVector SearchVector { get; set; }
        public bool Done { get; set; }
        public bool Deleted { get; set; }
        public ToDoList ToDoList { get; set; }
        public int? ToDoListId { get; set; }
    }
}
