using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocSchedule.API.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("username")]
        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Column("password_hash")]
        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Column("role")]
        [Required]
        [MaxLength(10)]
        public string Role { get; set; } = string.Empty;

        [Column("doctor_id")]
        public int? DoctorId { get; set; }

        [ForeignKey(nameof(DoctorId))]
        public Doctor? Doctor { get; set; }

        [Column("created_at")]
        public DateTime? CreatedAt { get; set; }
    }
}