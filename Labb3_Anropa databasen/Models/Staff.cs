using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Labb3_Anropa_databasen.Models
{
    [Table("Staff")]
    [Index(nameof(Phone), Name = "UQ__Staff__5C7E359E1F75380C", IsUnique = true)]
    [Index(nameof(Email), Name = "UQ__Staff__A9D10534EFCFB200", IsUnique = true)]
    [Index(nameof(Ssn), Name = "UQ__Staff__CA1E8E3CE2E4A978", IsUnique = true)]
    public partial class Staff
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; } = null!;
        [StringLength(50)]
        public string LastName { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string Position { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string EmploymentDate { get; set; } = null!;
        [Column("SSN")]
        [StringLength(10)]
        [Unicode(false)]
        public string Ssn { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string Phone { get; set; } = null!;
        [StringLength(320)]
        [Unicode(false)]
        public string? Email { get; set; }
    }
}
