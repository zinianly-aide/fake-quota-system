namespace FakeQuotaSystem.Models
{
    public class EmplQuotaSummary
    {
        public int TotalRegions { get; set; }
        public int TotalEmpvlTypes { get; set; }
        public int TotalApplications { get; set; }
        public int ActiveApplications { get; set; }
        public int PendingApprovals { get; set; }
    }

    public class EmplvlSummary
    {
        public int TotalEmployees { get; set; }
        public int TotalCertificates { get; set; }
        public int TotalQuotaDays { get; set; }
    }
}
