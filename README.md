# ScamShield AI

منصة لاكتشاف الاحتيال الرقمي والتصيد الإلكتروني باللغة العربية.
Backend: ASP.NET Core MVC (.NET 8) + Entity Framework Core (SQLite)
Frontend: Bootstrap 5 (RTL)

## طريقة التشغيل في Visual Studio

1. افتحي **Visual Studio 2022** (تأكدي من تثبيت workload: **ASP.NET and web development**).
2. من القائمة: `File > Open > Project/Solution` واختاري ملف `ScamShieldAI.csproj`.
3. عند أول فتح، Visual Studio سيسترجع الحزم (Packages) تلقائيًا (NuGet Restore).
   - إذا ما استرجعت تلقائيًا: كليك يمين على المشروع > Restore NuGet Packages.
4. اضغطي F5 أو زر التشغيل (▶) لتشغيل المشروع.
5. سيفتح المتصفح تلقائيًا على الصفحة الرئيسية.

قاعدة البيانات (SQLite ملف `scamshield.db`) تُنشأ تلقائيًا أول مرة تشغّلين فيها المشروع،
وتُعبّأ بالكلمات المفتاحية الأساسية تلقائيًا (لا تحتاجين أي إعداد يدوي).

## هيكل المشروع

```
ScamShieldAI/
├── Controllers/       -> Home, Analyze, Dashboard
├── Models/             -> AnalyzedMessage (Messages), KeywordRule (Keywords), AppUser (Users)
├── Data/               -> AppDbContext + DbSeeder (تعبئة الكلمات المفتاحية)
├── Services/           -> ScamAnalyzerService (محرك التحليل Rule-Based)
├── Views/              -> صفحات Home / Analyze / Result / History / Dashboard
└── wwwroot/            -> CSS
```

## كيف يعمل التحليل حاليًا (Level 1 - Rule Based)

- يقارن نص الرسالة بجدول الكلمات المفتاحية (`Keywords`) وكل كلمة لها وزن (Weight) محدد مسبقًا.
- يستخرج الروابط (URLs) وعناوين IP من النص (IOC Detection) باستخدام Regex.
- يفحص كل رابط: هل يستخدم HTTPS؟ هل هو رابط اختصار (bit.ly ...الخ)؟ هل يبدو مشبوهًا؟
- يجمع كل هذا في **درجة خطورة من 0 إلى 100** ويصنّف الرسالة: Safe / Suspicious / Scam.
- يحفظ كل رسالة تم تحليلها في قاعدة البيانات (تظهر في صفحة History ولوحة التحكم Dashboard).

## التطوير المستقبلي (Level 2 - Machine Learning)

الكود مبني بحيث `IScamAnalyzerService` هي الواجهة المستخدمة في الـ Controllers.
لاحقًا تقدرين تسوين كلاس جديد مثلاً `MlScamAnalyzerService` يطبق نفس الواجهة لكن
يستخدم نموذج تصنيف (مثلاً عبر ML.NET) بدل الكلمات المفتاحية فقط، وتستبدلينه في
Program.cs بسطر واحد:

```csharp
builder.Services.AddScoped<IScamAnalyzerService, MlScamAnalyzerService>();
```

## ملاحظة عن SQL Server

المشروع حاليًا يستخدم **SQLite** لتسهيل التشغيل المباشر بدون تثبيت أي سيرفر قواعد بيانات.
إذا احتجتي SQL Server (حسب التصميم الأصلي):
1. في `ScamShieldAI.csproj` استبدلي `Microsoft.EntityFrameworkCore.Sqlite` بـ `Microsoft.EntityFrameworkCore.SqlServer`.
2. في `Program.cs` استبدلي `UseSqlite(...)` بـ `UseSqlServer(...)`.
3. في `appsettings.json` عدّلي `DefaultConnection` لتكون connection string لسيرفرك.
