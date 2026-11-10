using NeuroPath.Core.Common;
using NeuroPath.Core.Models.Domain.Gamification;

namespace NeuroPath.Tests.Common;

public class UserTimeTests
{
    private static readonly TimeZoneInfo Tbilisi = UserTime.Resolve("Asia/Tbilisi");

    [Fact]
    public void LocalDate_LateUtcEvening_IsNextDayInTbilisi()
    {
        var utc = new DateTime(2026, 11, 10, 21, 0, 0, DateTimeKind.Utc);

        Assert.Equal(new DateTime(2026, 11, 11), UserTime.LocalDate(utc, Tbilisi));
        Assert.Equal(new DateTime(2026, 11, 10), UserTime.LocalDate(utc, TimeZoneInfo.Utc));
    }

    [Fact]
    public void StartOfLocalDayUtc_ReturnsLocalMidnightInUtc()
    {
        var utc = new DateTime(2026, 11, 10, 21, 30, 0, DateTimeKind.Utc);

        Assert.Equal(new DateTime(2026, 11, 10, 20, 0, 0), UserTime.StartOfLocalDayUtc(utc, Tbilisi));
    }

    [Theory]
    [InlineData("Asia/Tbilisi", true)]
    [InlineData("UTC", true)]
    [InlineData("Mars/Olympus", false)]
    [InlineData("", false)]
    public void IsValidTimeZone_RecognisesKnownZones(string id, bool expected)
        => Assert.Equal(expected, UserTime.IsValidTimeZone(id));

    [Theory]
    [InlineData(0, 0, 5)]
    [InlineData(1, 0, 5)]
    [InlineData(2, 0, 0)]
    [InlineData(2, 1, 5)]
    [InlineData(3, 1, 0)]
    public void EffectiveStreak_DependsOnDaysSinceLastActivity(int daysAgo, int freezes, int expected)
    {
        var now = new DateTime(2026, 11, 10, 12, 0, 0, DateTimeKind.Utc);
        var streak = new Streak { CurrentStreak = 5, LastActivityDate = now.AddDays(-daysAgo), StreakFreezeCount = freezes };

        Assert.Equal(expected, UserTime.EffectiveStreak(streak, now, TimeZoneInfo.Utc));
    }
}
