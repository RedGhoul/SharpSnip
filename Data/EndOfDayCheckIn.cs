using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Snips.Data
{
    public class EndOfDayCheckIn : AuditableEntity
    {
        public int Id { get; set; }
        public string Comments { get; set; }
        public string WhatWentWell { get; set; }
        public string WhatWentBad { get; set; }
        public bool Deleted { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
        [NotMapped]
        public NpgsqlTsVector SearchVector { get; set; }
    }
}
