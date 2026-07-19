using System.Text.RegularExpressions;
using ScamShieldAI.Data;
using ScamShieldAI.Models;

namespace ScamShieldAI.Services
{
    public class ScamAnalyzerService : IScamAnalyzerService
    {
        private readonly AppDbContext _context;

        // نطاقات معروفة بتقصير الروابط - وجودها لا يعني احتيال 100% لكنه مؤشر يرفع الشك
        private static readonly string[] ShortenerDomains =
        {
            "bit.ly", "tinyurl.com", "goo.gl", "t.co", "is.gd", "ow.ly", "cutt.us", "shorturl.at"
        };

        private static readonly Regex UrlRegex = new(
            @"(https?:\/\/[^\s]+|www\.[^\s]+)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex IpRegex = new(
            @"\b(?:\d{1,3}\.){3}\d{1,3}\b",
            RegexOptions.Compiled);

        public ScamAnalyzerService(AppDbContext context)
        {
            _context = context;
        }

        public AnalysisResultViewModel Analyze(string message)
        {
            var result = new AnalysisResultViewModel
            {
                OriginalMessage = message
            };

            int score = 0;
            var reasons = new List<string>();

            // 1) فحص الكلمات المفتاحية من قاعدة البيانات
            var keywordRules = _context.Keywords.ToList();
            foreach (var rule in keywordRules)
            {
                if (message.Contains(rule.Keyword, StringComparison.OrdinalIgnoreCase))
                {
                    score += rule.Weight;
                    reasons.Add($"وجود عبارة \"{rule.Keyword}\" ({rule.Category})");
                }
            }

            // 2) استخراج الروابط
            var urls = UrlRegex.Matches(message).Select(m => m.Value.TrimEnd('.', ',', ')')).Distinct().ToList();
            result.ExtractedUrls = urls;

            if (urls.Any())
            {
                score += 30; // وجود رابط بحد ذاته مؤشر خطورة (حسب جدول النقاط في التصميم)
                reasons.Add("وجود رابط داخل الرسالة");

                foreach (var url in urls)
                {
                    var check = CheckUrl(url);
                    result.UrlChecks.Add(check);

                    if (!check.IsHttps)
                    {
                        score += 10;
                        reasons.Add($"الرابط {url} لا يستخدم HTTPS");
                    }
                    if (check.IsShortener)
                    {
                        score += 10;
                        reasons.Add($"الرابط {url} يستخدم خدمة اختصار روابط");
                    }
                }
            }

            // 3) استخراج عناوين IP (IOC Detection)
            var ips = IpRegex.Matches(message).Select(m => m.Value).Distinct().ToList();
            result.ExtractedIps = ips;
            if (ips.Any())
            {
                score += 15;
                reasons.Add("وجود عنوان IP داخل الرسالة (مؤشر اختراق محتمل)");
            }

            // سقف الدرجة 100
            score = Math.Min(score, 100);

            result.RiskScore = score;
            result.Reasons = reasons;

            if (score >= 70)
            {
                result.RiskLevel = "مرتفع";
                result.ResultLabel = "Scam";
            }
            else if (score >= 35)
            {
                result.RiskLevel = "متوسط";
                result.ResultLabel = "Suspicious";
            }
            else
            {
                result.RiskLevel = "منخفض";
                result.ResultLabel = "Safe";
            }

            // حفظ النتيجة في قاعدة البيانات (History + Dashboard)
            var record = new AnalyzedMessage
            {
                Content = message,
                RiskScore = score,
                Result = result.ResultLabel,
                ThreatType = reasons.Any() ? string.Join(" | ", reasons.Take(3)) : "لا يوجد",
                MatchedKeywords = string.Join(",", keywordRules
                    .Where(r => message.Contains(r.Keyword, StringComparison.OrdinalIgnoreCase))
                    .Select(r => r.Keyword)),
                ExtractedUrls = string.Join(",", urls),
                ExtractedIps = string.Join(",", ips),
                Date = DateTime.Now
            };

            _context.Messages.Add(record);
            _context.SaveChanges();

            return result;
        }

        private UrlCheckInfo CheckUrl(string url)
        {
            bool isHttps = url.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
            bool isShortener = ShortenerDomains.Any(d => url.Contains(d, StringComparison.OrdinalIgnoreCase));

            // مؤشر بسيط: نطاقات كثيرة الشرطات/الأرقام تُعتبر مشبوهة أكثر (heuristic بسيط وليس قاطع)
            bool looksSuspicious = Regex.IsMatch(url, @"\d{3,}") || url.Count(c => c == '-') >= 3;

            return new UrlCheckInfo
            {
                Url = url,
                IsHttps = isHttps,
                IsShortener = isShortener,
                LooksSuspicious = looksSuspicious
            };
        }
    }
}
