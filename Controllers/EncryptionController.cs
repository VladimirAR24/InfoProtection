using InfoProtection.Models.ViewModels;
using InfoProtection.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using InfoProtection.Protection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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
            //Шифруем
            if (action == "encrypt")
            {
                if (model.Algorithm == "Кузнечик")
                {
                    model.TextEnd = Kuznechik.KuznechikEncrypt(model.TextStart);
                }
                else if (model.Algorithm == "RSA 16384")
                {
                    model.TextEnd = RSA.RsaEncrypt(model.TextStart);
                }
            }
            //АнтиШифруем
            else if (action == "decrypt")
            {
                if (model.Algorithm == "Кузнечик")
                {
                    model.TextEnd = Kuznechik.KuznechikDecrypt(model.TextStart);
                }
                else if (model.Algorithm == "RSA 16384")
                {
                    model.TextEnd = RSA.RsaDecrypt(model.TextStart);
                }
            }

            // Вернуть модель с результатом
            ModelState.Clear();
            return View(model);
        }

        [HttpGet]
        [Route("myencryptions")]
        public async Task<IActionResult> MyEncryptions()
        {
            // Получаем текущего пользователя
            var currentUser = User.Identity.Name;

            // Получаем шифры текущего пользователя из базы данных
            var userEncryptions = await _context.EncryptedMessages
                .Where(e => e.User.Username == currentUser)
                .ToListAsync();

            return View(userEncryptions);
        }

    }
}
