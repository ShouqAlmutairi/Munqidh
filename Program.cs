using Microsoft.EntityFrameworkCore;
using ScamShieldAI.Data;
using ScamShieldAI.Services;

var builder = WebApplication.CreateBuilder(args);

// إضافة MVC
builder.Services.AddControllersWithViews();

// إضافة قاعدة البيانات (SQLite افتراضيًا)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// تسجيل خدمة التحليل
builder.Services.AddScoped<IScamAnalyzerService, ScamAnalyzerService>();

var app = builder.Build();

// إنشاء قاعدة البيانات وتعبئتها بالكلمات المفتاحية عند أول تشغيل
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSeeder.Seed(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
