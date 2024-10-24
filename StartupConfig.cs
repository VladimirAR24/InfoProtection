using InfoProtection.Protection;
using InfoProtection.Servises;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class StartupConfig
{
    public IConfiguration Configuration { get; }

    public StartupConfig(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<JwtOptions>(Configuration.GetSection(nameof(JwtOptions)));

        // Добавление подключения к базе данных (PostgreSQL через Entity Framework Core)
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)  // добавление сервисов аутентификации = Bearer
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("secretkeysecretkeysecretkeysecretkeysecretkey"))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                context.Token = context.Request.Cookies["JwtToken"];

                return Task.CompletedTask;
            }
        };
    });      // подключение аутентификации с помощью jwt-токенов

        services.AddSingleton<IAuthorizationMiddlewareResultHandler, MyAuthorizationMiddlewareResultHandler>();     // В случае плохих мальчиков

        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Login";
        });

        services.AddAuthorization();            // добавление сервисов авторизации

        // Добавление MVC или контроллеров с представлениями
        services.AddControllersWithViews();
        Console.WriteLine("Config 1 OK");

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
