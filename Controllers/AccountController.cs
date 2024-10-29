using InfoProtection.Models;
using InfoProtection.Models.ViewModels;
using InfoProtection.Protection;
using InfoProtection.Servises;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
   


    public AccountController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Отображение страницы регистрации
    [HttpGet]
    [Route("register")] // Настройка маршрута для страницы регистрации
    public IActionResult Register()
    {
        return View(); // Возвращаем представление страницы регистрации
    }

    // Обработка данных формы регистрации
    [HttpPost]
    [Route("register")] // Настройка маршрута для страницы регистрации
    [ValidateAntiForgeryToken] // Защита от CSRF атак
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // Если модель не валидна, возвращаем пользователя на страницу регистрации
            return View(model);
        }
        DbAccess dbAccess = new DbAccess(_context);
        await dbAccess.CreateUserAsync(model);

        // Перенаправление на страницу авторизации
        return RedirectToAction("Encryption", "Encryption");
    }

    // Отображение страницы авторизации
    [HttpGet]
    [Route("login")]
    [AllowAnonymous]
    public IActionResult Login(string returnUrl = null)
    {
        if (User.Identity.IsAuthenticated) { return RedirectToAction("Encryption", "Encryption"); }
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }


    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        DbAccess dbAccess = new DbAccess(_context);

        var token = await dbAccess.Login(model);         // Генерация JWT токена
        if (token == null) { return View(model); }
        // Добавление токена в куки
        Response.Cookies.Append("JwtToken", token, new CookieOptions
        {
            HttpOnly = true, // Установка HttpOnly, чтобы токен не был доступен через JavaScript
            Expires = DateTime.UtcNow.AddHours(12), // Время жизни токена в куки
            Secure = true,  // Устанавливается true, если работаете с HTTPS
            SameSite = SameSiteMode.Strict // Повышает безопасность куки, не отправляя их с кросс-сайтовыми запросами
        });

        return RedirectToAction("Encryption", "Encryption"); // Редирект на главную страницу после успешного логина
    }

    [HttpPost]
    [Route("Logout")]
    public async Task<IActionResult> Logout()
    {
        return RedirectToAction("Login");
    }

    [HttpGet]
    [Route("AdminPage")]
    public IActionResult AdminPage()
    {
        return View();
    }

    // Обработка данных формы регистрации
    [HttpPost]
    [Route("AdminPage")]
    [ValidateAntiForgeryToken] // Защита от CSRF атак
    public async Task<IActionResult> AdminUserCreate(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);

        }
        DbAccess dbAccess = new DbAccess(_context);
        await dbAccess.CreateUserAsync(model);
        //bool userCreated = await CreateUserAsync(model);

        //if (userCreated)
        //{
        //    return RedirectToAction("SuccessPage");
        //}
        //else
        //{
        //    ModelState.AddModelError("", "Пользователь с таким именем уже существует.");
        //}
        // Перенаправление на страницу авторизации
        return RedirectToAction("AdminPage", "Account");
        // Если модель не валидна, возвращаем пользователя на страницу регистрации
    }
}
