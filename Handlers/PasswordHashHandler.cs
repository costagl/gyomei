namespace Gyomei.Handlers;

using BCrypt.Net;

public static class PasswordHashHandler
{
    public static string HashPassword(string password) => BCrypt.HashPassword(password);

    public static bool VerifyPassword(string password, string hash) => BCrypt.Verify(password, hash);
}