using InfoProtection.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using InfoProtection.Protection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using InfoProtection.Servises;
using InfoProtection.Models;

namespace InfoProtection.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Encryption(EncryptionViewModel model, string action)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


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
                    //model.TextEnd = RSA.RsaDecrypt(model.TextStart, null);
                }
            }

            DbAccess dbAccess = new DbAccess(_context);
            string currentUserId = User.Claims.FirstOrDefault().Value;
            await dbAccess.CreateEnc(model, currentUserId);


            // Вернуть модель с результатом
            ModelState.Clear();
            return View(model);
        }

        [HttpGet]
        [Route("myencryptions")]
        public async Task<IActionResult> MyEncryptions()
        {
            // Вытаскиваем из клеймов юзерайдишник
            string currentUserId = User.Claims.FirstOrDefault().Value;

            // Получаем шифры текущего пользователя из базы данных
            DbAccess dbAccess = new DbAccess(_context);
            var userEncryptions = await dbAccess.GetAllEncById(currentUserId);
            //var userEncryptions = await _context.EncryptedMessages                .Where(e => e.User.Username == currentUser)                .ToListAsync();

            return View(userEncryptions);
        }

    }
}
