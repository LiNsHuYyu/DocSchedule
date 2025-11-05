using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocSchedule.API.Models
{
    [Table("shifts")]
    public class Schedule
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("sub_department_id")]
        [Required]
        public int SubDepartmentId { get; set; }

        [Column("shift_date", TypeName = "date")]
        [Required]
        public DateTime ShiftDate { get; set; }

        [Column("doctor_id")]
        public int? DoctorId { get; set; }

        [Column("status")]
        [MaxLength(10)]
        public string Status { get; set; } = "Draft";

        [ForeignKey(nameof(SubDepartmentId))]
        public SubDepartment? SubDepartment { get; set; }

        [ForeignKey(nameof(DoctorId))]
        public Doctor? Doctor { get; set; }
    }
}