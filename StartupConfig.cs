using InfoProtection.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class StartupConfig
{
    public IConfiguration Configuration { get; }

    public StartupConfig(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        
        // Добавление подключения к базе данных (PostgreSQL через Entity Framework Core)
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

        services.AddIdentity<IdentityUser, IdentityRole>()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // Добавление MVC или контроллеров с представлениями
        services.AddControllersWithViews();
        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Login"; // Путь к странице входа
        });
        Console.WriteLine("Config 1 OK");

        // Добавление сервисов для работы с авторизацией
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.LoginPath = "/Login"; // Путь на страницу логина
        });

        services.AddAuthorization(); // Добавление поддержки авторизации

    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext dbContext)
    {

        // Если приложение запущено в режиме разработки (Development), то показываем страницу ошибок
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            // Для боевой среды - выводим кастомную страницу ошибок
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts(); // Настройка HSTS для безопасности
        }

        // Настройка HTTPS перенаправления - перенаправление всех HTTP-запросов на HTTPS для безопасности.
        app.UseHttpsRedirection();

        // Настройка статических файлов (css, js, изображения и т.д.)
        // разрешает приложение обслуживать статические файлы, такие как CSS, JS, изображения.
        app.UseStaticFiles();

        // Применение миграций автоматически
        dbContext.Database.Migrate();

        //включает маршрутизацию запросов к контроллерам.
        app.UseRouting();

        // Middleware аутентификации
        app.UseAuthentication();

        // Middleware авторизации
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}"); // Установка страницы логина как главной
        });

        Console.WriteLine("Config 2 OK");
    }
}
