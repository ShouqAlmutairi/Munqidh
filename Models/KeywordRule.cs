using System.ComponentModel.DataAnnotations;

namespace ScamShieldAI.Models
{
    // جدول Keywords في قاعدة البيانات
    public class KeywordRule
    {
        [Key] // <-- أضيفي هذا السطر لتحديد المفتاح الأساسي للجدول
        public int KeywordId { get; set; }

        [Required]
        public string Keyword { get; set; } = string.Empty;

        public int Weight { get; set; } // النقاط

        public string Category { get; set; } = "عام"; // ترهيب / استعجال / طلب بيانات / مالي ... الخ
    }
}