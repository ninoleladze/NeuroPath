using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Enums;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Models.DTOs.Social;
using NeuroPath.Core.Services;
using NeuroPath.Tests.Infrastructure;

namespace NeuroPath.Tests.Services;

public class FriendshipServiceTests : IDisposable
{
    private readonly TestDatabase _database = new();

    public void Dispose() => _database.Dispose();

    [Fact]
    public async Task SendRequest_ToSelf_IsRejected()
    {
        await TestData.AddUserAsync(_database.Context, "alice");

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            new FriendshipService(_database.Context).SendRequestAsync(new SendFriendRequestDto { ReceiverId = "alice" }, "alice"));
    }

    [Fact]
    public async Task SendRequest_WhenReversePending_AcceptsInsteadOfDuplicating()
    {
        await TestData.AddUserAsync(_database.Context, "alice");
        await TestData.AddUserAsync(_database.Context, "bob");
        var service = new FriendshipService(_database.Context);

        await service.SendRequestAsync(new SendFriendRequestDto { ReceiverId = "bob" }, "alice");
        var accepted = await service.SendRequestAsync(new SendFriendRequestDto { ReceiverId = "alice" }, "bob");

        await using var check = _database.NewContext();
        Assert.True(accepted);
        Assert.Equal(1, await check.Friendships.CountAsync());
        Assert.Equal(1, await check.FriendRequests.CountAsync());
        Assert.Equal(FriendRequestStatus.Accepted, (await check.FriendRequests.SingleAsync()).Status);
        Assert.True(await service.AreFriendsAsync("bob", "alice"));
    }

    [Fact]
    public async Task SendRequest_WhenAlreadyFriends_IsRejected()
    {
        await TestData.AddUserAsync(_database.Context, "alice");
        await TestData.AddUserAsync(_database.Context, "bob");
        var service = new FriendshipService(_database.Context);
        await service.SendRequestAsync(new SendFriendRequestDto { ReceiverId = "bob" }, "alice");
        await service.SendRequestAsync(new SendFriendRequestDto { ReceiverId = "alice" }, "bob");

        await Assert.ThrowsAsync<ConflictException>(() =>
            service.SendRequestAsync(new SendFriendRequestDto { ReceiverId = "bob" }, "alice"));
    }

    [Fact]
    public async Task SendRequest_DuplicatePending_ReturnsFalse()
    {
        await TestData.AddUserAsync(_database.Context, "alice");
        await TestData.AddUserAsync(_database.Context, "bob");
        var service = new FriendshipService(_database.Context);

        Assert.True(await service.SendRequestAsync(new SendFriendRequestDto { ReceiverId = "bob" }, "alice"));
        Assert.False(await service.SendRequestAsync(new SendFriendRequestDto { ReceiverId = "bob" }, "alice"));
    }
}
