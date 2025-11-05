using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocSchedule.API.Models
{
    [Table("sub_departments")]
    public class SubDepartment
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("department_id")]
        [Required]
        public int DepartmentId { get; set; }

        [Column("name")]
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [ForeignKey(nameof(DepartmentId))]
        public Department? Department { get; set; }
    }
}