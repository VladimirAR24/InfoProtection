using InfoProtection.Models;
using System.ComponentModel.DataAnnotations.Schema;

[Table("users", Schema = "public")]
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }
    public string Role { get; set; }
    public string Email { get; set; }

    // Навигационные свойства для связи с другими таблицами
    public ICollection<EncryptedMessage> EncryptedMessages { get; set; }
    public ICollection<NfaCode> NfaCodes { get; set; }
}
