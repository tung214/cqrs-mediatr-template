namespace Laborie.Service.Application.Exceptions;

public sealed class ValidationException(IReadOnlyDictionary<string, string[]> errorsDictionary)
: ApplicationException("Lỗi xác thực dữ liệu", "Dữ liệu đầu vào không đúng!!!")
{

    public IReadOnlyDictionary<string, string[]> ErrorsDictionary { get; } = errorsDictionary;
}
