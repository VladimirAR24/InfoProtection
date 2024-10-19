using InfoProtection.Models;
using InfoProtection.Models.ViewModels;
using InfoProtection.Protection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;


    public AccountController(ApplicationDbContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _signInManager = signInManager;
        _userManager = userManager;
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
    [Route("Login")]
    public IActionResult Login(string returnUrl = null)
    {
        return View(
            //new LoginViewModel { ReturnUrl = returnUrl }
            );
    }


    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            View(model);
        }

        
        return View(model);
    }

    [HttpPost]
    [Route("Logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
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
