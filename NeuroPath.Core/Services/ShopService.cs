using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.DTOs.Gamification;
using NeuroPath.Data.Context;

namespace NeuroPath.Core.Services;

public class ShopService : IShopService
{
    private readonly NeuroPathDbContext _db;

    public ShopService(NeuroPathDbContext db) => _db = db;

    public async Task<List<ShopItemDto>> GetItemsAsync()
        => await _db.ShopItems.Where(i => i.IsActive)
            .Select(i => new ShopItemDto { Id = i.Id, Name = i.Name, Description = i.Description, Price = i.Price, ImageUrl = i.ImageUrl, Category = i.Category, StockQuantity = i.StockQuantity, IsActive = i.IsActive })
            .ToListAsync();

    public async Task<bool> PurchaseAsync(int itemId, string userId)
    {
        var item = await _db.ShopItems.FindAsync(itemId);
        if (item == null || !item.IsActive) return false;
        var user = await _db.Users.FindAsync(userId);
        if (user == null || user.CurrentBalance < item.Price) return false;
        if (item.StockQuantity.HasValue && item.StockQuantity <= 0) return false;

        user.CurrentBalance -= item.Price;
        if (item.StockQuantity.HasValue) item.StockQuantity--;
        _db.UserInventories.Add(new Models.Domain.Gamification.UserInventory { UserId = userId, ShopItemId = itemId });
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<UserInventoryDto>> GetInventoryAsync(string userId)
        => await _db.UserInventories.Where(i => i.UserId == userId)
            .Include(i => i.ShopItem)
            .Select(i => new UserInventoryDto { Id = i.Id, ShopItemId = i.ShopItemId, ShopItemName = i.ShopItem.Name, PurchasedAt = i.PurchasedAt, IsEquipped = i.IsEquipped })
            .ToListAsync();

    public async Task<bool> EquipItemAsync(int inventoryId, string userId)
    {
        var item = await _db.UserInventories.FirstOrDefaultAsync(i => i.Id == inventoryId && i.UserId == userId);
        if (item == null) return false;
        item.IsEquipped = !item.IsEquipped;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<StreakDto?> GetStreakAsync(string userId)
    {
        var s = await _db.Streaks.FirstOrDefaultAsync(x => x.UserId == userId);
        if (s == null) return null;
        return new StreakDto { CurrentStreak = s.CurrentStreak, LongestStreak = s.LongestStreak, LastActivityDate = s.LastActivityDate, TotalActiveDays = s.TotalActiveDays, StreakFreezeCount = s.StreakFreezeCount };
    }
}
