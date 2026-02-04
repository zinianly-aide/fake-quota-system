using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FakeQuotaSystem.Models
{
    [Table("TNA_TBL_EMPLQUOTA")]
    public class EmplQuota
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [MaxLength(3)]
        public string RegionId { get; set; }

        [Required]
        public int QuotaSeqNo { get; set; }

        [Required]
        [MaxLength(4)]
        public int Year { get; set; }

        [Required]
        [MaxLength(3)]
        [StringLength(2)]
        public string ApplicationType { get; set; }

        [Required]
        [MaxLength(10)]
        public decimal DayAmount { get; set; }

        [Required]
        [MaxLength(10)]
        public decimal HourAmount { get; set; }

        [Required]
        [MaxLength(10)]
        public decimal QuotaDayAmount { get; set; }

        [Required]
        [MaxLength(10)]
        public decimal QuotaHourAmount { get; set; }

        [Required]
        [MaxLength(50)]
        public string Remarks { get; set; }
    }

    [Table("TNA_TBL_EMPVL")]
    public class Empvl
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [MaxLength(2)]
        [StringLength(2)]
        public string EmpId { get; set; }

        [Required]
        [MaxLength(30)]
        public string ActivityName { get; set; }

        [Required]
        [MaxLength(30)]
        [StringLength(2)]
        public string ActivityDay { get; set; }

        [Required]
        [MaxLength(100)]
        [Certificate]
        public string Certificate { get; set; }

        [Required]
        [MaxLength(30)]
        public string CreateEmpId { get; set; }

        [Required]
        public string CreateEmpName { get; set; }

        [Required]
        [MaxLength(30)]
        public DateTime CreateDate { get; set; }

        [Required]
        [MaxLength(30)]
        public string UpdateEmpId { get; set; }

        [Required]
        [MaxLength(30)]
        public string UpdateEmpName { get; set; }

        [Required]
        [MaxLength(30)]
        public DateTime UpdateDate { get; set; }

        [Required]
        [MaxLength(10)]
        public int ApplyQuotaDays { get; set; }

        [Required]
        [MaxLength(10)]
        public string Rdptaskid { get; set; }

        [Required]
        [MaxLength(100)]
        public string Rdppnodeaccount { get; set; }

        [Required]
        [MaxLength(100)]
        public string Rdppnodenameber { get; set; }

        [Required]
        [MaxLength(100)]
        public string Rdppid { get; set; }

        [Required]
        [MaxLength(100)]
        public string Rdppreviewers { get; set; }

        [Required]
        [MaxLength(10)]
        public string Rdppid { get; set; }
    }
}
