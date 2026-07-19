using Microsoft.AspNetCore.Mvc;
using ScamShieldAI.Data;
using ScamShieldAI.Services;

namespace ScamShieldAI.Controllers
{
    public class AnalyzeController : Controller
    {
        private readonly IScamAnalyzerService _analyzer;
        private readonly AppDbContext _context;

        public AnalyzeController(IScamAnalyzerService analyzer, AppDbContext context)
        {
            _analyzer = analyzer;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string messageText)
        {
            if (string.IsNullOrWhiteSpace(messageText))
            {
                ModelState.AddModelError(string.Empty, "الرجاء إدخال نص الرسالة أولاً");
                return View();
            }

            var result = _analyzer.Analyze(messageText);
            return View("Result", result);
        }

        public IActionResult History()
        {
            var messages = _context.Messages
                .OrderByDescending(m => m.Date)
                .Take(100)
                .ToList();

            return View(messages);
        }
    }
}
