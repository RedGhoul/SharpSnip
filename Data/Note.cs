using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Snips.Data
{
    public class Note : AuditableEntity
    {
        public Note()
        {
            this.Deleted = false;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string CodeContent { get; set; }
        public string CodeLanguage { get; set; }
        public bool HasCode { get; set; }
        public bool Deleted { get; set; }
        [NotMapped]
        public NpgsqlTsVector SearchVector { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
