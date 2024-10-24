
using System.Text;

namespace InfoProtection.Protection
{
    //static мб
    public static class HashMethods
    {
        public static string GenerateSalt()
        {
            throw new NotImplementedException();
        }

        public static string HashPasswordUsingStreebog(string password, string salt)
        {
            throw new NotImplementedException();
        }

        public static bool Verify(string password, string passwordHash)
        {
            return true;
        }
    }
}
