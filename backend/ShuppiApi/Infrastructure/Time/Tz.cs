namespace ShuppiApi.Infrastructure.Time;

public static class Tz
{
  public static readonly TimeZoneInfo Tokyo = Resolve();

  // DateTime → UTC（Unspecified は JST とみなす）
  public static DateTime ToUtc(DateTime dt) => dt.Kind switch
  {
    DateTimeKind.Utc         => dt,
    DateTimeKind.Local       => dt.ToUniversalTime(),
    DateTimeKind.Unspecified => TimeZoneInfo.ConvertTimeToUtc(dt, Tokyo),
    _ => dt
  };

  // UTC → JST
  public static DateTime FromUtc(DateTime utc) =>
    TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc), Tokyo);

  // ローカル暦日の開始(含む)をUTCへ
  public static DateTime DayStartUtc(DateOnly d) =>
    TimeZoneInfo.ConvertTimeToUtc(d.ToDateTime(TimeOnly.MinValue, DateTimeKind.Unspecified), Tokyo);

  // ローカル暦日の翌日00:00(含まない)をUTCへ
  public static DateTime DayEndExclusiveUtc(DateOnly d) =>
    TimeZoneInfo.ConvertTimeToUtc(d.AddDays(1).ToDateTime(TimeOnly.MinValue, DateTimeKind.Unspecified), Tokyo);

  private static TimeZoneInfo Resolve()
  {
    try {
      return TimeZoneInfo.FindSystemTimeZoneById(
        OperatingSystem.IsWindows() ? "Tokyo Standard Time" : "Asia/Tokyo");
    }
    catch (Exception ex) {
      throw new InvalidOperationException("Tokyo timezone not found. Install tzdata.", ex);
    }
  }
}
