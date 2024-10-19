using InfoProtection.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    private User (int Id, string Username, string PasswordHash, string Salt, string Role, string Email)
    {
        this.Id = Id;
        this.Username = Username;
        this.PasswordHash = PasswordHash;
        this.Salt = Salt;
        this.Role = Role;
        this.Email = Email;
    }
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }
    public string Role { get; set; }
    public string Email { get; set; }
    public static User Create (int Id, string Username, string PasswordHash, string Salt, string Role, string Email)
    {
        return new User(Id, Username, PasswordHash, Salt, Role, Email);
    }

    // Навигационные свойства для связи с другими таблицами
    public ICollection<EncryptedMessage> EncryptedMessages { get; set; }
    public ICollection<NfaCode> NfaCodes { get; set; }
}
