namespace FgQuota.Api.Models;

public static class QuotaConstants
{
    public const decimal HoursPerDay = 8m;

    public static readonly Dictionary<string, string> TypeNameMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["SYP"] = "赡养人陪护假",
        ["DSZ"] = "独生子女陪护假",
        ["BJH"] = "北京护理假",
        ["SZH"] = "深圳护理假"
    };

    public static readonly Dictionary<string, decimal> TypeQuotaMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["SYP"] = 5,
        ["DSZ"] = 7,
        ["BJH"] = 10,
        ["SZH"] = 15
    };
}
