using NeuroPath.Core.Models.DTOs.Gamification;

namespace NeuroPath.Core.Interfaces;

public interface IShopService
{
    Task<List<ShopItemDto>> GetItemsAsync();
    Task<bool> PurchaseAsync(int itemId, string userId);
    Task<List<UserInventoryDto>> GetInventoryAsync(string userId);
    Task<bool> EquipItemAsync(int inventoryId, string userId);
    Task<StreakDto?> GetStreakAsync(string userId);
}
