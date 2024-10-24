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
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }


    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        DbAccess dbAccess = new DbAccess(_context);
        if (!ModelState.IsValid)
        {
            View(model);
        }
        string token = await dbAccess.Login(model);
        
        return View();
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
