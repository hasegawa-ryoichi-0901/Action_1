using System;
using System.Runtime.CompilerServices;

public class DateUtils {
    public static DateTimeOffset NowDate => DateTimeOffset.Now;

    public static string Now =>
        DateUtils.NowDate.ToOffset(DateTimeOffset.Now.Offset).ToString(GameConstants.yyyyMMddHHmmss);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ToUtcString(DateTimeOffset date) {
        return date.UtcDateTime.ToString(GameConstants.yyyyMMddHHmmss);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsInRange(DateTimeOffset from, DateTimeOffset to) =>
        IsInRange(from, to, NowDate);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsInRange(DateTimeOffset from, DateTimeOffset to, DateTimeOffset target) {
        return from == DateTimeOffset.MinValue ||
               to == DateTimeOffset.MinValue ||
               from <= target && target <= to;
    }

    public static DateTimeOffset DecodeTime(string time, bool useOffset) {
        if (string.IsNullOrEmpty(time)) {
            return DateTimeOffset.MinValue;
        }

        var year = time.Substring(0, 4);
        var month = time.Substring(4, 2);
        var day = time.Substring(6, 2);
        var hour = time.Substring(8, 2);
        var minute = time.Substring(10, 2);
        var sec = time.Substring(12, 2);
        var offset = useOffset ? DateUtils.NowDate.Offset : TimeSpan.Zero;

        return new DateTimeOffset(
            int.Parse(year),
            int.Parse(month),
            int.Parse(day),
            int.Parse(hour),
            int.Parse(minute),
            int.Parse(sec),
            offset);
    }
}