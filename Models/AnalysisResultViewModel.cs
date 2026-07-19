namespace ScamShieldAI.Models
{
    public class AnalysisResultViewModel
    {
        public string OriginalMessage { get; set; } = string.Empty;
        public int RiskScore { get; set; }
        public string RiskLevel { get; set; } = string.Empty; // منخفض / متوسط / مرتفع
        public string ResultLabel { get; set; } = string.Empty; // Safe / Suspicious / Scam
        public List<string> Reasons { get; set; } = new();
        public List<string> ExtractedUrls { get; set; } = new();
        public List<string> ExtractedIps { get; set; } = new();
        public List<UrlCheckInfo> UrlChecks { get; set; } = new();
    }

    public class UrlCheckInfo
    {
        public string Url { get; set; } = string.Empty;
        public bool IsHttps { get; set; }
        public bool IsShortener { get; set; }
        public bool LooksSuspicious { get; set; }
    }

    public class DashboardViewModel
    {
        public int TotalMessages { get; set; }
        public int ScamCount { get; set; }
        public int SuspiciousCount { get; set; }
        public int SafeCount { get; set; }
        public List<KeyValuePair<string, int>> TopKeywords { get; set; } = new();
    }
}
