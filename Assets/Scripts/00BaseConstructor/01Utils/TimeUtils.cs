using System;

public static class TimeUtils
{
    private static DateTime _startTime = new DateTime(1970, 1, 1, 0, 0, 0);

    public static long GetUnixDateTimeSeconds(DateTime dateTime)
    {
        TimeSpan timeSpan = dateTime - _startTime;
        return Convert.ToInt64(timeSpan.TotalSeconds);
    }
}
