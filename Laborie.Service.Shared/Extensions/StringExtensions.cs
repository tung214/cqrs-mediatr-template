using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Laborie.Service.Shared.Extensions
{
    public static class StringExtensions
    {
        const string AllowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public static IEnumerable<string> NextRandomStrings(int length, int count)
        {
            Random rnd = new();
            var usedRandomStrings = new HashSet<string>();
            char[] chars = new char[length];
            int setLength = AllowedChars.Length;

            while (count-- > 0)
            {
                int stringLength = length;

                for (int i = 0; i < stringLength; ++i)
                {
                    chars[i] = AllowedChars[rnd.Next(setLength)];
                }

                string randomString = new(chars, 0, stringLength);

                if (usedRandomStrings.Add(randomString))
                {
                    yield return randomString;
                }
                else
                {
                    count++;
                }
            }
        }
        

        public static string GenerateSaltedHash(string input, string salt)
        {
            // Kết hợp input với salt
            var saltedInput = $"{salt}{input}";

            // Tạo SHA256 và chuyển đổi chuỗi thành byte
            using (var sha256 = SHA256.Create())
            {
                var saltedInputBytes = Encoding.UTF8.GetBytes(saltedInput);
                var hashBytes = sha256.ComputeHash(saltedInputBytes);

                // Chuyển đổi hash bytes thành chuỗi hexadecimal
                var hashString = BitConverter.ToString(hashBytes).Replace("-", "");
                return hashString;
            }
        }


        public static string GetDataFromToken(this string token, string claim)
        {
            if (string.IsNullOrEmpty(token))
            {
                return "";
            }
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(token);
            var tokenClaims = jsonToken.Claims.ToList();
            var data = tokenClaims.Where(x => x.Type == claim).FirstOrDefault();
            return data != null ? data.Value : "";
        }
    }
}