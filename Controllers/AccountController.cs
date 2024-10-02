using InfoProtection.Models;
using InfoProtection.Models.ViewModels;
using InfoProtection.Protection;
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
        if (ModelState.IsValid)
        {
            DbAccess dbAccess = new DbAccess(_context);
            await dbAccess.CreateUserAsync(model);
            
            // Перенаправление на страницу авторизации
            return RedirectToAction("Encryption", "Encryption");
        }

        // Если модель не валидна, возвращаем пользователя на страницу регистрации
        return View(model);
    }

    // Отображение страницы авторизации
    [HttpGet]
    [Route("login")]
    public IActionResult Login()
    {
        return View(); // Возвращаем представление страницы авторизации
    }

    // Обработка данных формы авторизации
    [HttpPost]
    [Route("login")]
    public IActionResult Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Логика аутентификации пользователя
            // Проверка введённого пароля с сохранённым хешем
            return RedirectToAction("Index", "Home"); // Перенаправление на главную страницу после успешного входа
        }

        return View(model); // Возвращаем форму авторизации с ошибками
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
        if (ModelState.IsValid)
        {
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
        }
        // Если модель не валидна, возвращаем пользователя на страницу регистрации
        return View(model);
    }
}
