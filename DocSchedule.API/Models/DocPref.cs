using System;   //為了DateTime可以用
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocSchedule.API.Models
{
    [Table("doc_prefs")]
    public class DocPref
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }    //MySQL bigint-> C# long

        [Column("doctor_id")]
        [Required]
        public int DoctorId { get; set; }

        [Column("pref_date", TypeName = "date")]
        [Required]
        public DateTime PrefDate { get; set; }

        [ForeignKey(nameof(DoctorId))]
        public Doctor? Doctor { get; set; }
    }
}