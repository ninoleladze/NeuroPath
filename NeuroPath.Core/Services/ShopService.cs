using Microsoft.EntityFrameworkCore;
using NeuroPath.Core.Common;
using NeuroPath.Core.Exceptions;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Models.Domain.Gamification;
using NeuroPath.Core.Models.DTOs.Gamification;

namespace NeuroPath.Core.Services;

public class ShopService : IShopService
{
    private readonly INeuroPathDbContext _db;

    public ShopService(INeuroPathDbContext db) => _db = db;

    public async Task<List<ShopItemDto>> GetItemsAsync()
        => await _db.ShopItems.AsNoTracking()
            .Where(i => i.IsActive)
            .OrderBy(i => i.Category).ThenBy(i => i.Price)
            .Select(i => new ShopItemDto
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                Price = i.Price,
                ImageUrl = i.ImageUrl,
                Category = i.Category,
                StockQuantity = i.StockQuantity,
                IsActive = i.IsActive
            })
            .ToListAsync();

    public async Task<List<ShopItemDto>> GetAllItemsAsync()
        => await _db.ShopItems.AsNoTracking()
            .OrderBy(i => i.Category).ThenBy(i => i.Name)
            .Select(i => new ShopItemDto
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                Price = i.Price,
                ImageUrl = i.ImageUrl,
                Category = i.Category,
                StockQuantity = i.StockQuantity,
                IsActive = i.IsActive
            })
            .ToListAsync();

    public async Task<ShopItemDto> CreateItemAsync(SaveShopItemDto dto)
    {
        var item = new ShopItem();
        Apply(item, dto);
        _db.ShopItems.Add(item);
        await _db.SaveChangesAsync();
        return ToDto(item);
    }

    public async Task<ShopItemDto?> UpdateItemAsync(int id, SaveShopItemDto dto)
    {
        var item = await _db.ShopItems.FirstOrDefaultAsync(i => i.Id == id);
        if (item == null) return null;
        Apply(item, dto);
        await _db.SaveChangesAsync();
        return ToDto(item);
    }

    public async Task<UserInventoryDto> PurchaseAsync(int itemId, string userId)
    {
        var item = await _db.ShopItems.AsNoTracking().FirstOrDefaultAsync(i => i.Id == itemId && i.IsActive)
            ?? throw new NotFoundException("Item not found.");

        await using var transaction = await _db.Database.BeginTransactionAsync();

        var stockReserved = await _db.ShopItems
            .Where(i => i.Id == itemId && (i.StockQuantity == null || i.StockQuantity > 0))
            .ExecuteUpdateAsync(s => s.SetProperty(i => i.StockQuantity, i => i.StockQuantity == null ? null : i.StockQuantity - 1));
        if (stockReserved == 0)
            throw new BusinessRuleException("This item is out of stock.");

        var charged = await _db.Users
            .Where(u => u.Id == userId && u.CurrentBalance >= item.Price)
            .ExecuteUpdateAsync(s => s.SetProperty(u => u.CurrentBalance, u => u.CurrentBalance - item.Price));
        if (charged == 0)
            throw new BusinessRuleException("You do not have enough points for this item.");

        var inventory = new UserInventory { UserId = userId, ShopItemId = itemId };
        _db.UserInventories.Add(inventory);
        await _db.SaveChangesAsync();
        await transaction.CommitAsync();

        return new UserInventoryDto { Id = inventory.Id, ShopItemId = itemId, ShopItemName = item.Name, PurchasedAt = inventory.PurchasedAt, IsEquipped = false };
    }

    public async Task<List<UserInventoryDto>> GetInventoryAsync(string userId)
        => await _db.UserInventories.AsNoTracking()
            .Where(i => i.UserId == userId)
            .OrderByDescending(i => i.PurchasedAt)
            .Select(i => new UserInventoryDto { Id = i.Id, ShopItemId = i.ShopItemId, ShopItemName = i.ShopItem.Name, PurchasedAt = i.PurchasedAt, IsEquipped = i.IsEquipped })
            .ToListAsync();

    public async Task<bool> SetEquippedAsync(int inventoryId, bool isEquipped, string userId)
        => await _db.UserInventories
            .Where(i => i.Id == inventoryId && i.UserId == userId)
            .ExecuteUpdateAsync(s => s.SetProperty(i => i.IsEquipped, isEquipped)) > 0;

    public async Task<StreakDto?> GetStreakAsync(string userId)
    {
        var streak = await _db.Streaks.AsNoTracking().FirstOrDefaultAsync(s => s.UserId == userId);
        if (streak == null) return null;

        var timeZoneId = await _db.UserSettings.Where(s => s.UserId == userId).Select(s => s.TimeZoneId).FirstOrDefaultAsync();
        return new StreakDto
        {
            CurrentStreak = UserTime.EffectiveStreak(streak, DateTime.UtcNow, UserTime.Resolve(timeZoneId)),
            LongestStreak = streak.LongestStreak,
            LastActivityDate = streak.LastActivityDate,
            TotalActiveDays = streak.TotalActiveDays,
            StreakFreezeCount = streak.StreakFreezeCount
        };
    }

    private static void Apply(ShopItem item, SaveShopItemDto dto)
    {
        item.Name = dto.Name;
        item.Description = dto.Description;
        item.Price = dto.Price;
        item.ImageUrl = dto.ImageUrl;
        item.Category = dto.Category;
        item.StockQuantity = dto.StockQuantity;
        item.IsActive = dto.IsActive;
    }

    private static ShopItemDto ToDto(ShopItem i) => new()
    {
        Id = i.Id,
        Name = i.Name,
        Description = i.Description,
        Price = i.Price,
        ImageUrl = i.ImageUrl,
        Category = i.Category,
        StockQuantity = i.StockQuantity,
        IsActive = i.IsActive
    };
}
