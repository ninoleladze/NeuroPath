using Microsoft.EntityFrameworkCore;
using NeuroPath.Tests.Infrastructure;

namespace NeuroPath.Tests.Data;

public class UtcDateTimeTests : IDisposable
{
    private readonly TestDatabase _database = new();

    public void Dispose() => _database.Dispose();

    [Fact]
    public async Task DateTimes_ReadFromDatabase_AreMarkedUtc()
    {
        await TestData.AddUserAsync(_database.Context, "player");
        await TestData.AddStreakAsync(_database.Context, "player", 3, new DateTime(2026, 11, 10, 8, 30, 0, DateTimeKind.Utc));

        await using var check = _database.NewContext();
        var user = await check.Users.SingleAsync(u => u.Id == "player");
        var streak = await check.Streaks.SingleAsync(s => s.UserId == "player");

        Assert.Equal(DateTimeKind.Utc, user.CreatedAt.Kind);
        Assert.Equal(DateTimeKind.Utc, streak.LastActivityDate.Kind);
        Assert.Equal(new DateTime(2026, 11, 10, 8, 30, 0, DateTimeKind.Utc), streak.LastActivityDate);
    }
}
