using InfoProtection.Models.ViewModels;
using InfoProtection.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace InfoProtection.Controllers
{
    public class EncryptionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EncryptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("encryption")]
        public IActionResult Encryption(EncryptionViewModel model)
        {

            return View(model);
        }

        [HttpPost]
        [Route("encryption")]
        public IActionResult Encryption(EncryptionViewModel model, string action)
        {

            if (action == "encrypt")
            {
                // В зависимости от выбранного алгоритма
                if (model.Algorithm == "Кузнечик")
                {
                    model.TextEnd = model.TextStart;
                }
                else if (model.Algorithm == "RSA 16384")
                {
                    model.TextEnd = model.TextStart;
                }
            }
            else if (action == "decrypt")
            {
                // Расшифровка
                if (model.Algorithm == "Кузнечик")
                {
                    model.TextEnd = model.TextStart;
                }
                else if (model.Algorithm == "RSA 16384")
                {
                    model.TextEnd = model.TextStart;
                }
            }
            

            // Вернуть модель с результатом
            return View(model);
        }

        // Пример методов для шифрования алгоритмом "Кузнечик" и "RSA"
        private string KuznechikEncrypt(string text)
        {
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(textBytes); // Заглушка
        }

        private string KuznechikDecrypt(string text)
        {
            byte[] textBytes = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(textBytes); // Заглушка
        }

        private string RsaEncrypt(string text)
        {
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(textBytes); // Заглушка
        }

        private string RsaDecrypt(string text)
        {
            byte[] textBytes = Convert.FromBase64String(text);
            return Encoding.UTF8.GetString(textBytes); // Заглушка
        }
    }
}
