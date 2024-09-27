namespace InfoProtection.Models
{
    public class EncryptedMessage
    {
        public int Id { get; set; }
        public int UserId { get; set; }  // Foreign key

        public string EncryptedMessageText { get; set; }
        public string Algorithm { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Навигационное свойство для связи с пользователем
        public User User { get; set; }
    }
}
