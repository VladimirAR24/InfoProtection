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
    [ValidateAntiForgeryToken] // Защита от CSRF атак
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            // 1. Проверяем, существует ли пользователь с таким именем
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (existingUser != null)
            {
                ModelState.AddModelError("", "Пользователь с таким именем уже существует.");
                return View(model);
            }

            // 2. Генерация соли
            string salt = HashMethods.GenerateSalt();

            // 3. Хеширование пароля с использованием Стрибог
            string hashedPassword = HashMethods.HashPasswordUsingStreebog(model.Password, salt);

            // 4. Создание нового пользователя
            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = hashedPassword,
                Salt = salt,
                Role = "User" // или другая роль по умолчанию
            };

            // 5. Сохранение пользователя в базе данных
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // 6. Перенаправление на страницу авторизации
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

    [HttpPost]
    public IActionResult AdminUserCreate()
    {

        return View();
    }
    public 
}
