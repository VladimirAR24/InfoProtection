using InfoProtection.Models.ViewModels;
using InfoProtection.Protection;
using InfoProtection.Servises;
using Microsoft.EntityFrameworkCore;

namespace InfoProtection.Models
{
    // TODO: Переименовать в юзерсервис и настроить логику, чтобы в контроллеры передавался он, а не appdbcontext
    public class DbAccess
    {
        private readonly ApplicationDbContext _context;

        public DbAccess(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateUserAsync(RegisterViewModel model)
        {
            // 1. Проверяем, существует ли пользователь с таким именем
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
            if (existingUser != null)
            {
                // Если пользователь уже существует, возвращаем false
                return false;
            }

            // 2. Генерация соли
            string salt = HashMethods.GenerateSalt();
            //string salt = "Какая-то соль";
            // 3. Хеширование пароля с использованием Стрибог
            //string hashedPassword = "Какой-то хэш";
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

            // Если всё прошло успешно, возвращаем true
            return true;
        }

        public async Task<string> Login(LoginViewModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null) { throw new Exception("Failed to login user dont exict"); }               //TODO Сделать return null

            var result = HashMethods.VerifyPassword(model.Password, user.PasswordHash, user.Salt);
            if (result == false) { throw new Exception("Failed to login cant unhash password"); };      //TODO Сделать return null

            var jwtProvider = new JwtProvider();
            string token = jwtProvider.GenerateToken(user);
            return token;
        }

        public async Task<bool> CreateEnc(EncryptionViewModel model, string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(userId));
            //Сюда как-то скинуть дату куррентюзера для fk
            var encryptedMessage = new EncryptedMessage
            {
                UserId = int.Parse(userId),
                Algorithm = model.Algorithm,
                OriginalText = model.TextStart,
                EncryptedText = model.TextEnd,
                EncryptionDate = DateTime.UtcNow,
                User = user
            };

            _context.EncryptedMessages.Add(encryptedMessage);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<EncryptedMessage>> GetAllEncById(string currentUserId)
        {
            var userEncryptions = await _context.EncryptedMessages                
                .Where(e => e.User.Id.ToString() == currentUserId)                
                .ToListAsync();
            return userEncryptions;
        }

        public async Task<List<EncryptedMessage>> GetAllEnc()
        {
            var encryptedMessages = await _context.EncryptedMessages.ToListAsync();
            return encryptedMessages;
        }
    }
}
