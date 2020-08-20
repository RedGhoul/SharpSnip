using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snips.Models
{
    public class NoteDTO
    {
        //Id = x.Id,
        //        Name = x.Name,
        //        HasCode = x.HasCode,
        //        CodeLanguage = x.CodeLanguage,
        //        Created = x.Created,
        //        LastModified = x.LastModified

        public int Id { get; set; }
        public string Name { get; set; }
        public bool HasCode { get; set; }
        public string CodeLanguage { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }
    }
}
