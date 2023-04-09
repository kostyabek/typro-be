using Typro.Application.Helpers;

namespace Typro.Infrastructure.Helpers;

public class NicknameHelper : INicknameHelper
{
    private const string Prefix = "user";

    public string GenerateNicknameFromDate(DateTime date)
        => $"{Prefix}{date.Year}{date.Month}{date.Day}{date.Hour}{date.Minute}{date.Second}{date.Millisecond / 10}";
}