using System.Text;

namespace Laborie.Service.Shared.Extensions;
public class PasswordGenerator
{
    private static readonly Random Random = new();

    public static string GeneratePassword(int length = 8)
    {
        if (length < 4)
            throw new ArgumentException("Password length must be at least 4 characters.");

        // Các ký tự cần thiết cho mật khẩu
        const string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        const string numberChars = "0123456789";
        const string specialChars = "!@#$%^&*()-_=+<>?";

        // Bắt buộc có ít nhất 1 ký tự từ mỗi nhóm
        var passwordChars = new StringBuilder()
            .Append(uppercaseChars[Random.Next(uppercaseChars.Length)])
            .Append(lowercaseChars[Random.Next(lowercaseChars.Length)])
            .Append(numberChars[Random.Next(numberChars.Length)])
            .Append(specialChars[Random.Next(specialChars.Length)]);

        // Thêm các ký tự ngẫu nhiên từ tất cả các nhóm cho đến khi đạt độ dài yêu cầu
        string allChars = uppercaseChars + lowercaseChars + numberChars + specialChars;
        for (int i = passwordChars.Length; i < length; i++)
        {
            passwordChars.Append(allChars[Random.Next(allChars.Length)]);
        }

        // Xáo trộn các ký tự để mật khẩu không theo thứ tự
        return new string(passwordChars.ToString().OrderBy(_ => Random.Next()).ToArray());
    }
}
