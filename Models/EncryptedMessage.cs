namespace InfoProtection.Models
{
    public class EncryptedMessage
    {
        public int Id { get; set; }
        public int UserId { get; set; }  // Foreign key
        public string Algorithm { get; set; }
        public string OriginalText { get; set; }
        public string EncryptedText { get; set; }
        public DateTime EncryptionDate { get; set; }


        // Навигационное свойство для связи с пользователем
        public User User { get; set; }
    }
}
