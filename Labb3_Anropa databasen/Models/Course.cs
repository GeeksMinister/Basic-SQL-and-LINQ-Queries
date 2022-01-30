using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Labb3_Anropa_databasen.Models
{
    [Keyless]
    public partial class Course
    {
        [Column("id")]
        public int Id { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column("Teacher1_Id")]
        public int Teacher1Id { get; set; }
        [Column("Teacher2_Id")]
        public int? Teacher2Id { get; set; }
        [Column("Teacher3_Id")]
        public int? Teacher3Id { get; set; }

        [ForeignKey(nameof(Teacher1Id))]
        public virtual Staff Teacher1 { get; set; } = null!;
        [ForeignKey(nameof(Teacher2Id))]
        public virtual Staff? Teacher2 { get; set; }
        [ForeignKey(nameof(Teacher3Id))]
        public virtual Staff? Teacher3 { get; set; }
    }
}
