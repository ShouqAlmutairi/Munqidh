using System.ComponentModel.DataAnnotations;

namespace ScamShieldAI.Models
{
    // جدول Messages في قاعدة البيانات
    public class AnalyzedMessage
    {
        [Key] // <-- أضيفي هذا السطر هنا لتحديد المفتاح الأساسي
        public int MessageId { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public int RiskScore { get; set; }

        public string Result { get; set; } = string.Empty; // Safe / Suspicious / Scam

        public string ThreatType { get; set; } = string.Empty; // نوع التهديد

        public string MatchedKeywords { get; set; } = string.Empty; // مفصولة بفواصل

        public string ExtractedUrls { get; set; } = string.Empty;

        public string ExtractedIps { get; set; } = string.Empty;

        public DateTime Date { get; set; } = DateTime.Now;
    }
}