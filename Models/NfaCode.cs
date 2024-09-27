using System;

namespace InfoProtection.Models
{
    public class NfaCode
    {
        public int Id { get; set; }
        public int UserId { get; set; }  // Foreign key

        public string OtpCode { get; set; }
        public DateTime ExpirationTime { get; set; }

        // Навигационное свойство для связи с пользователем
        public User User { get; set; }
    }

}
