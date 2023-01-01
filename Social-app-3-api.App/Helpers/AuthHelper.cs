using Microsoft.AspNetCore.Identity;
using Social_app_3_api.Model.User;

namespace Social_app_3_api.Helpers
{
    public static class AuthHelper
    {
        public static string GenerateHash(User user, string password)
        {
            PasswordHasher<User> passwordHasher= new();

            string hashedPassword = passwordHasher.HashPassword(user, password);

            return hashedPassword;
        }
        public static bool VerifyHash(User user, string password, string hashedPassword)
        {
            PasswordHasher<User> passwordHasher = new();

            PasswordVerificationResult isValid = passwordHasher.VerifyHashedPassword(user, hashedPassword, password);
            // ajustar método para possibilidar a criação de novo hash quando necessário.
            if(isValid.ToString() == "Failed")
            {
                return false;
            }

            return true;
        }
    }
}
