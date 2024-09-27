using InfoProtection.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    // Отображение страницы регистрации
    [HttpGet]
    [Route("register")] // Настройка маршрута для страницы регистрации
    public IActionResult Register()
    {
        return View(); // Возвращаем представление страницы регистрации
    }

    // Обработка данных формы регистрации
    [HttpPost]
    [Route("register")] // Маршрут для отправки данных регистрации
    public IActionResult Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Логика регистрации пользователя, хеширование пароля и сохранение в БД
            // Валидация и создание пользователя
            return RedirectToAction("Login"); // Перенаправление на страницу авторизации после успешной регистрации
        }

        return View(model); // Если что-то не так, возвращаем форму с ошибками
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
}
