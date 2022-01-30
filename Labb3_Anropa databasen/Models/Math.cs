using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Labb3_Anropa_databasen.Models
{
    [Keyless]
    [Table("Math")]
    public partial class Math
    {
        public int StudentId { get; set; }
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        public int? TeacherId { get; set; }
        [StringLength(2)]
        [Unicode(false)]
        public string? Grade { get; set; }
        [Column(TypeName = "date")]
        public DateTime? GradeSubmitted { get; set; }
    }
}
