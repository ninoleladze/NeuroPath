using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using NeuroPath.Core.Models.Domain.Identity;
using NeuroPath.Core.Models.DTOs.File;
using NeuroPath.Core.Models.DTOs.Gamification;
using NeuroPath.Tests.Infrastructure;

namespace NeuroPath.Tests.Http;

public class FilesAndShopTests : IClassFixture<ApiFactory>
{
    private static readonly byte[] Png = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A, 0x00, 0x00, 0x00, 0x0D, 0x49, 0x48, 0x44, 0x52 };

    private readonly ApiFactory _factory;

    public FilesAndShopTests(ApiFactory factory) => _factory = factory;

    [Fact]
    public async Task Avatar_UploadIsServedSafelyAndReplacesThePreviousOne()
    {
        var account = await _factory.CreateAccountAsync();
        using var client = _factory.ClientFor(account.AccessToken);

        var first = await UploadAsync(client, "/api/files/avatar", Png, "me.png", "image/png");
        Assert.Equal(HttpStatusCode.OK, first.StatusCode);
        var firstFile = (await first.Content.ReadFromJsonAsync<FileResponseDto>())!;
        Assert.Equal("image/png", firstFile.ContentType);

        using var served = await _factory.CreateClient().GetAsync(firstFile.FileUrl);
        Assert.Equal(HttpStatusCode.OK, served.StatusCode);
        Assert.Equal("nosniff", served.Headers.GetValues("X-Content-Type-Options").Single());
        Assert.Equal("inline", served.Content.Headers.ContentDisposition?.DispositionType);
        Assert.Equal(Png, await served.Content.ReadAsByteArrayAsync());

        var second = await UploadAsync(client, "/api/files/avatar", Png, "again.png", "image/png");
        var secondFile = (await second.Content.ReadFromJsonAsync<FileResponseDto>())!;

        var mine = (await client.GetFromJsonAsync<List<FileResponseDto>>("/api/files"))!;
        Assert.Equal(secondFile.Id, Assert.Single(mine).Id);
        Assert.Equal(HttpStatusCode.NotFound, (await _factory.CreateClient().GetAsync(firstFile.FileUrl)).StatusCode);
        var me = await client.GetFromJsonAsync<Dictionary<string, object?>>("/api/users/me");
        Assert.Equal(secondFile.FileUrl, me!["avatarUrl"]?.ToString());
    }

    [Theory]
    [InlineData("fake.png", "image/png", "not really an image")]
    [InlineData("script.html", "text/html", "<script>alert(1)</script>")]
    [InlineData("empty.png", "image/png", "")]
    public async Task Avatar_RejectsSpoofedOrDisallowedFiles(string fileName, string contentType, string content)
    {
        var account = await _factory.CreateAccountAsync();
        using var client = _factory.ClientFor(account.AccessToken);

        var response = await UploadAsync(client, "/api/files/avatar", System.Text.Encoding.UTF8.GetBytes(content), fileName, contentType);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Empty((await client.GetFromJsonAsync<List<FileResponseDto>>("/api/files"))!);
    }

    [Fact]
    public async Task Files_CanOnlyBeDeletedByTheirOwner()
    {
        var owner = await _factory.CreateAccountAsync();
        var other = await _factory.CreateAccountAsync();
        using var ownerClient = _factory.ClientFor(owner.AccessToken);
        using var otherClient = _factory.ClientFor(other.AccessToken);
        var upload = await UploadAsync(ownerClient, "/api/files/avatar", Png, "me.png", "image/png");
        var file = (await upload.Content.ReadFromJsonAsync<FileResponseDto>())!;

        Assert.Equal(HttpStatusCode.NotFound, (await otherClient.DeleteAsync($"/api/files/{file.Id}")).StatusCode);
        Assert.Equal(HttpStatusCode.OK, (await _factory.CreateClient().GetAsync(file.FileUrl)).StatusCode);

        Assert.Equal(HttpStatusCode.NoContent, (await ownerClient.DeleteAsync($"/api/files/{file.Id}")).StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, (await _factory.CreateClient().GetAsync(file.FileUrl)).StatusCode);
    }

    [Fact]
    public async Task Shop_PurchaseChargesTheBalanceAndTheItemCanBeEquipped()
    {
        var admin = await _factory.LoginAdminAsync();
        var buyer = await _factory.CreateAccountAsync();
        using var adminClient = _factory.ClientFor(admin.AccessToken);
        using var buyerClient = _factory.ClientFor(buyer.AccessToken);
        await SetBalanceAsync(buyer.Id, 100);

        var created = await adminClient.PostAsJsonAsync("/api/shop/items", new SaveShopItemDto
        {
            Name = $"Theme {Guid.NewGuid():N}"[..20],
            Description = "Dark theme",
            Price = 60,
            Category = "Cosmetic",
            StockQuantity = 1
        });
        Assert.Equal(HttpStatusCode.OK, created.StatusCode);
        var item = (await created.Content.ReadFromJsonAsync<ShopItemDto>())!;

        var purchase = await buyerClient.PostAsync($"/api/shop/items/{item.Id}/purchase", null);
        Assert.Equal(HttpStatusCode.OK, purchase.StatusCode);
        var owned = (await purchase.Content.ReadFromJsonAsync<UserInventoryDto>())!;

        Assert.Equal(HttpStatusCode.BadRequest, (await buyerClient.PostAsync($"/api/shop/items/{item.Id}/purchase", null)).StatusCode);

        var equip = await buyerClient.PutAsJsonAsync($"/api/shop/inventory/{owned.Id}/equipped", new SetEquippedDto { IsEquipped = true });
        Assert.Equal(HttpStatusCode.NoContent, equip.StatusCode);
        var inventory = (await buyerClient.GetFromJsonAsync<List<UserInventoryDto>>("/api/shop/inventory"))!;
        Assert.True(Assert.Single(inventory).IsEquipped);

        var me = await buyerClient.GetFromJsonAsync<Dictionary<string, object?>>("/api/users/me");
        Assert.Equal("40", me!["currentBalance"]?.ToString());
    }

    [Fact]
    public async Task Shop_CannotEquipSomeoneElsesItem()
    {
        var admin = await _factory.LoginAdminAsync();
        var buyer = await _factory.CreateAccountAsync();
        var other = await _factory.CreateAccountAsync();
        using var adminClient = _factory.ClientFor(admin.AccessToken);
        using var buyerClient = _factory.ClientFor(buyer.AccessToken);
        using var otherClient = _factory.ClientFor(other.AccessToken);
        await SetBalanceAsync(buyer.Id, 100);

        var created = await adminClient.PostAsJsonAsync("/api/shop/items", new SaveShopItemDto { Name = $"Badge {Guid.NewGuid():N}"[..20], Description = "Badge", Price = 10, Category = "Cosmetic" });
        var item = (await created.Content.ReadFromJsonAsync<ShopItemDto>())!;
        var owned = (await (await buyerClient.PostAsync($"/api/shop/items/{item.Id}/purchase", null)).Content.ReadFromJsonAsync<UserInventoryDto>())!;

        var response = await otherClient.PutAsJsonAsync($"/api/shop/inventory/{owned.Id}/equipped", new SetEquippedDto { IsEquipped = true });

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private static async Task<HttpResponseMessage> UploadAsync(HttpClient client, string url, byte[] bytes, string fileName, string contentType)
    {
        var file = new ByteArrayContent(bytes);
        file.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        using var form = new MultipartFormDataContent { { file, "file", fileName } };
        return await client.PostAsync(url, form);
    }

    private async Task SetBalanceAsync(string userId, int balance)
    {
        await using var scope = _factory.Services.CreateAsyncScope();
        var users = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var user = (await users.FindByIdAsync(userId))!;
        user.CurrentBalance = balance;
        await users.UpdateAsync(user);
    }
}
