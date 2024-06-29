using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace Loony.Tools
{
    public class Security
    {
        public static string SHA256(string password)
        {
            UnicodeEncoding uEncode = new UnicodeEncoding();
            byte[] passwordByte = uEncode.GetBytes(password);
            SHA256Managed sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(passwordByte);
            return Convert.ToBase64String(hash);
        }

        public static byte[] GetRandomSalt(int length)
        {
            var random = new RNGCryptoServiceProvider();
            byte[] salt = new byte[length];
            random.GetNonZeroBytes(salt);
            return salt;
        }

        public static byte[] SaltHashPassword(byte[] password, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();
            byte[] plainTextWithSaltBytes = new byte[password.Length + salt.Length];
            for (int i = 0; i < password.Length; i++)
                plainTextWithSaltBytes[i] = password[i];
            for (int i = 0; i < salt.Length; i++)
                plainTextWithSaltBytes[password.Length + i] = salt[i];
            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

        public static string CreatePassword(int length)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
            string password = "";
            Random rnd = new Random();
            for (int i = 0; i < length; i++)
            {
                int random = rnd.Next(chars.Length);
                password = password + chars[random].ToString();
            }

            return password;
        }

        #region Token

        public static string GetToken(string id)
        {
            var symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(id));
            var signingCredentials = new SigningCredentials(symetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                issuer: "LoonyToken",
                audience: id,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: signingCredentials
                ); ;

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static bool ValidateToken(string id, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = new TokenValidationParameters()
                {
                    ValidateLifetime = true,
                    ValidIssuer = "LoonyToken",
                    ValidAudience = id,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(id))
                };

                SecurityToken validatedToken;
                IPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

    }
}
