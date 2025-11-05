using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocSchedule.API.Models
{
    [Table("doctors")]
    public class Doctor
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Column("phone")]
        [MaxLength(30)]
        public string? Phone { get; set; }

        [Column("sub_department_id")]
        [Required]
        public int SubDepartmentId { get; set; }

        [ForeignKey(nameof(SubDepartmentId))]
        public SubDepartment? SubDepartment { get; set; }
        public User? User { get; set; }
        public ICollection<Schedule>? Schedule { get; set; }
        public ICollection<DocPref>? DocPref { get; set; }
    }
}