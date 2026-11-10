using NeuroPath.Core.Models.Domain.Gamification;

namespace NeuroPath.Core.Common;

public static class UserTime
{
    public const string DefaultTimeZoneId = "UTC";

    public static bool IsValidTimeZone(string? timeZoneId)
        => !string.IsNullOrWhiteSpace(timeZoneId) && TimeZoneInfo.TryFindSystemTimeZoneById(timeZoneId, out _);

    public static TimeZoneInfo Resolve(string? timeZoneId)
        => !string.IsNullOrWhiteSpace(timeZoneId) && TimeZoneInfo.TryFindSystemTimeZoneById(timeZoneId, out var zone)
            ? zone
            : TimeZoneInfo.Utc;

    public static DateTime LocalDate(DateTime utc, TimeZoneInfo zone)
        => TimeZoneInfo.ConvertTimeFromUtc(DateTime.SpecifyKind(utc, DateTimeKind.Utc), zone).Date;

    public static DateTime StartOfLocalDayUtc(DateTime utcNow, TimeZoneInfo zone)
    {
        var localMidnight = DateTime.SpecifyKind(LocalDate(utcNow, zone), DateTimeKind.Unspecified);
        while (zone.IsInvalidTime(localMidnight))
            localMidnight = localMidnight.AddMinutes(30);
        return TimeZoneInfo.ConvertTimeToUtc(localMidnight, zone);
    }

    public static int EffectiveStreak(Streak streak, DateTime utcNow, TimeZoneInfo zone)
    {
        if (streak.CurrentStreak <= 0) return 0;

        var today = LocalDate(utcNow, zone);
        var lastDay = LocalDate(streak.LastActivityDate, zone);

        if (lastDay >= today.AddDays(-1)) return streak.CurrentStreak;
        if (lastDay == today.AddDays(-2) && streak.StreakFreezeCount > 0) return streak.CurrentStreak;
        return 0;
    }
}
