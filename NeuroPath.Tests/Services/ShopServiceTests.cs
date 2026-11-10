using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Models.Domain.Gamification;
using NeuroPath.Core.Services;
using NeuroPath.Tests.Infrastructure;

namespace NeuroPath.Tests.Services;

public class ShopServiceTests : IDisposable
{
    private readonly TestDatabase _database = new();

    public void Dispose() => _database.Dispose();

    private async Task<ShopItem> AddItemAsync(int price, int? stock)
    {
        var item = new ShopItem { Name = "Theme", Description = "Dark theme", Price = price, Category = "Cosmetic", StockQuantity = stock, IsActive = true };
        _database.Context.ShopItems.Add(item);
        await _database.Context.SaveChangesAsync();
        return item;
    }

    [Fact]
    public async Task Purchase_WithEnoughBalance_ChargesUserAndDecrementsStock()
    {
        await TestData.AddUserAsync(_database.Context, "buyer", balance: 100);
        var item = await AddItemAsync(price: 40, stock: 3);

        var result = await new ShopService(_database.Context).PurchaseAsync(item.Id, "buyer");

        await using var check = _database.NewContext();
        Assert.Equal(item.Id, result.ShopItemId);
        Assert.Equal(60, (await check.Users.SingleAsync(u => u.Id == "buyer")).CurrentBalance);
        Assert.Equal(2, (await check.ShopItems.SingleAsync(i => i.Id == item.Id)).StockQuantity);
        Assert.Equal(1, await check.UserInventories.CountAsync(i => i.UserId == "buyer"));
    }

    [Fact]
    public async Task Purchase_WithInsufficientBalance_ChangesNothing()
    {
        await TestData.AddUserAsync(_database.Context, "buyer", balance: 10);
        var item = await AddItemAsync(price: 40, stock: 3);

        await Assert.ThrowsAsync<BusinessRuleException>(() => new ShopService(_database.Context).PurchaseAsync(item.Id, "buyer"));

        await using var check = _database.NewContext();
        Assert.Equal(10, (await check.Users.SingleAsync(u => u.Id == "buyer")).CurrentBalance);
        Assert.Equal(3, (await check.ShopItems.SingleAsync(i => i.Id == item.Id)).StockQuantity);
        Assert.Equal(0, await check.UserInventories.CountAsync());
    }

    [Fact]
    public async Task Purchase_OutOfStock_IsRejected()
    {
        await TestData.AddUserAsync(_database.Context, "buyer", balance: 100);
        var item = await AddItemAsync(price: 10, stock: 0);

        await Assert.ThrowsAsync<BusinessRuleException>(() => new ShopService(_database.Context).PurchaseAsync(item.Id, "buyer"));

        await using var check = _database.NewContext();
        Assert.Equal(100, (await check.Users.SingleAsync(u => u.Id == "buyer")).CurrentBalance);
    }

    [Fact]
    public async Task Purchase_UnlimitedStock_KeepsStockNull()
    {
        await TestData.AddUserAsync(_database.Context, "buyer", balance: 100);
        var item = await AddItemAsync(price: 10, stock: null);

        await new ShopService(_database.Context).PurchaseAsync(item.Id, "buyer");

        await using var check = _database.NewContext();
        Assert.Null((await check.ShopItems.SingleAsync(i => i.Id == item.Id)).StockQuantity);
    }
}
