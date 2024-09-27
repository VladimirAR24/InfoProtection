using InfoProtection.Models;
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

        // Добавление MVC или контроллеров с представлениями
        services.AddControllersWithViews();
        Console.WriteLine("Config 1 OK");
        // Добавление сервисов для работы с авторизацией
        // services.AddAuthentication();
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
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        Console.WriteLine("Config 2 OK");
    }
}
