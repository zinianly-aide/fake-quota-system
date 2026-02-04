using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FakeQuotaSystem.Models
{
    [Table("TNA_TBL_EMPLQUOTA")]
    public class EmplQuota
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RegionId { get; set; }

        [Required]
        [StringLength(2)]
        public string Year { get; set; }

        [Required]
        [StringLength(4)]
        public string ApplicationType { get; set; }

        [Required]
        [Range(1, 100000, Minimum = 0)]
        public decimal DayAmount { get; set; }

        [Required]
        [Range(1, 100000, Minimum = 0)]
        public decimal HourAmount { get; set; }

        [Required]
        [Range(1, 100000, Minimum = 0)]
        public decimal QuotaDayAmount { get; set; }

        [Required]
        [Range(1, 100000, Minimum = 0)]
        public decimal QuotaHourAmount { get; set; }

        [Table("TNA_TBL_EMPVL")]
        public class Empl
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int EmpId { get; set; }

            [Required]
            [StringLength(32)]
            public string EmpIdString { get; set; }

            [Required]
            [StringLength(30)]
            public string ActivityName { get; set; }

            [Required]
            [StringLength(30)]
            public string ActivityDay { get; set; }

            [Required]
            [StringLength(100)]
            public string JoinCertificate { get; set; }

            [Required]
            [StringLength(30)]
            public string Status { get; set; }

            [Required]
            [StringLength(30)]
            public string CreateEmpId { get; set; }

            [Required]
            [StringLength(30)]
            public string CreateEmpName { get; set; }

            [Required]
            [StringLength(30)]
            public string CreatedAt { get; set; }

            [Required]
            [StringLength(30)]
            public string UpdatedAt { get; set; }

            [Required]
            [StringLength(10)]
            public int ApplyQuotaDays { get; set; }

            [Required]
            [StringLength(100)]
            public string RdppTaskId { get; set; }

            [Required]
            [StringLength(100)]
            public string RdppNodeAccount { get; set; }

            [Required]
            [StringLength(100)]
            public string RdppNodenumber { get; set; }

            [Required]
            [StringLength(100)]
            public string RdppId { get; set; }

            [Required]
            [StringLength(100)]
            public string RdppViewers { get; set; }

            [Required]
            [StringLength(10)]
            public string ApplyType { get; set; }
        }
    }
}
