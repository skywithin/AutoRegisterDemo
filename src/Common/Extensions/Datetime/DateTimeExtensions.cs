namespace Common.Extensions.Datetime;

public static class DateTimeExtensions
{
    public static string ToHHMMSS(this DateTime dateTime) =>
        dateTime.ToString("HH:mm:ss");
}
