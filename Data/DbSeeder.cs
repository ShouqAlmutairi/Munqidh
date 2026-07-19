using ScamShieldAI.Models;

namespace ScamShieldAI.Data
{
    public static class DbSeeder
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Keywords.Any())
            {
                var keywords = new List<KeywordRule>
                {
                    new() { Keyword = "حسابك موقوف", Weight = 25, Category = "ترهيب" },
                    new() { Keyword = "تم إيقاف حسابك", Weight = 25, Category = "ترهيب" },
                    new() { Keyword = "تم تجميد حسابك", Weight = 25, Category = "ترهيب" },
                    new() { Keyword = "حدث بياناتك", Weight = 30, Category = "طلب بيانات" },
                    new() { Keyword = "تحديث بياناتك", Weight = 30, Category = "طلب بيانات" },
                    new() { Keyword = "بيانات حسابك", Weight = 20, Category = "طلب بيانات" },
                    new() { Keyword = "اضغط هنا", Weight = 20, Category = "رابط مشبوه" },
                    new() { Keyword = "فوراً", Weight = 15, Category = "استعجال" },
                    new() { Keyword = "فورا", Weight = 15, Category = "استعجال" },
                    new() { Keyword = "خلال 24 ساعة", Weight = 15, Category = "استعجال" },
                    new() { Keyword = "سيتم إيقاف الخدمة", Weight = 20, Category = "ترهيب" },
                    new() { Keyword = "تحويل مالي", Weight = 20, Category = "مالي" },
                    new() { Keyword = "جائزة", Weight = 20, Category = "طعم / إغراء" },
                    new() { Keyword = "هدية", Weight = 15, Category = "طعم / إغراء" },
                    new() { Keyword = "مخالفة", Weight = 15, Category = "انتحال جهة" },
                    new() { Keyword = "مخالفة مرورية", Weight = 20, Category = "انتحال جهة" },
                    new() { Keyword = "أبشر", Weight = 10, Category = "انتحال جهة" },
                    new() { Keyword = "الراجحي", Weight = 10, Category = "انتحال جهة" },
                    new() { Keyword = "شحنة", Weight = 10, Category = "انتحال جهة" },
                    new() { Keyword = "كلمة المرور", Weight = 25, Category = "طلب بيانات" },
                    new() { Keyword = "رقم البطاقة", Weight = 30, Category = "طلب بيانات" },
                    new() { Keyword = "رمز التحقق", Weight = 30, Category = "طلب بيانات" },
                };

                context.Keywords.AddRange(keywords);
                context.SaveChanges();
            }
        }
    }
}
