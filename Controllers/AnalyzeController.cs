using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // مضاف لعمل استعلام السجل بشكل غير متزامن
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

        // 1. تم تحويل هذا الـ Action ليعمل بشكل غير متزامن Async
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string messageText)
        {
            if (string.IsNullOrWhiteSpace(messageText))
            {
                ModelState.AddModelError(string.Empty, "الرجاء إدخال نص الرسالة أولاً");
                return View();
            }

            // استدعاء الدالة المحدثة باستخدام await
            var result = await _analyzer.AnalyzeAsync(messageText);
            return View("Result", result);
        }

        // 2. تحسين دالة السجل لتصبح غير متزامنة ToListAsync لحماية أداء الصفحة عند كثرة البيانات
        public async Task<IActionResult> History()
        {
            var messages = await _context.Messages
                .OrderByDescending(m => m.Date)
                .Take(100)
                .ToListAsync();

            return View(messages);
        }
    }
}