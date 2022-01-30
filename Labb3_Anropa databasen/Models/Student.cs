using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Labb3_Anropa_databasen.Models
{
    [Table("Student")]
    [Index(nameof(Email), Name = "UQ__Student__A9D10534BA4F068B", IsUnique = true)]
    [Index(nameof(Ssn), Name = "UQ__Student__CA1E8E3C109A6281", IsUnique = true)]
    public partial class Student
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; } = null!;
        [StringLength(50)]
        public string LastName { get; set; } = null!;
        [Column("SSN")]
        [StringLength(10)]
        [Unicode(false)]
        public string Ssn { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string? Class { get; set; }
        [StringLength(320)]
        [Unicode(false)]
        public string? Email { get; set; }
    }
}
