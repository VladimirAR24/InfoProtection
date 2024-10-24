namespace InfoProtection.Protection
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } = "secretkeysecretkeysecretkeysecretkeysecretkey";
        public int ExpiredHoures { get; set; } = 5;
    }
}
