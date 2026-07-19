using Microsoft.AspNetCore.Mvc;
using ScamShieldAI.Data;
using ScamShieldAI.Models;

namespace ScamShieldAI.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var messages = _context.Messages.ToList();

            var vm = new DashboardViewModel
            {
                TotalMessages = messages.Count,
                ScamCount = messages.Count(m => m.Result == "Scam"),
                SuspiciousCount = messages.Count(m => m.Result == "Suspicious"),
                SafeCount = messages.Count(m => m.Result == "Safe"),
            };

            // أكثر الكلمات استخداماً
            var keywordCounts = new Dictionary<string, int>();
            foreach (var msg in messages)
            {
                if (string.IsNullOrWhiteSpace(msg.MatchedKeywords)) continue;

                foreach (var kw in msg.MatchedKeywords.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    if (!keywordCounts.ContainsKey(kw)) keywordCounts[kw] = 0;
                    keywordCounts[kw]++;
                }
            }

            vm.TopKeywords = keywordCounts
                .OrderByDescending(k => k.Value)
                .Take(10)
                .ToList();

            return View(vm);
        }
    }
}
