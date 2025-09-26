namespace aspnetcore_hexagonal_api.Shared.Utilities;

public static class DateTimeHelper
{
    public static DateTime UtcNow => DateTime.UtcNow;

    public static DateTime ToUtc(DateTime dateTime)
    {
        return dateTime.Kind switch
        {
            DateTimeKind.Utc => dateTime,
            DateTimeKind.Local => dateTime.ToUniversalTime(),
            DateTimeKind.Unspecified => DateTime.SpecifyKind(dateTime, DateTimeKind.Utc),
            _ => dateTime
        };
    }

    public static DateTime? ToUtc(DateTime? dateTime)
    {
        return dateTime?.ToUniversalTime();
    }

    public static string ToIso8601String(DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
    }

    public static DateTime StartOfDay(DateTime dateTime)
    {
        return dateTime.Date;
    }

    public static DateTime EndOfDay(DateTime dateTime)
    {
        return dateTime.Date.AddDays(1).AddTicks(-1);
    }

    public static bool IsValidDateRange(DateTime? startDate, DateTime? endDate)
    {
        if (!startDate.HasValue || !endDate.HasValue)
            return true;

        return startDate.Value <= endDate.Value;
    }

    public static int GetAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;

        if (birthDate.Date > today.AddYears(-age))
            age--;

        return age;
    }
}