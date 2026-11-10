using NeuroPath.Core.Models.DTOs.Gamification;

namespace NeuroPath.Core.Interfaces;

public interface IShopService
{
    Task<List<ShopItemDto>> GetItemsAsync();
    Task<List<ShopItemDto>> GetAllItemsAsync();
    Task<ShopItemDto> CreateItemAsync(SaveShopItemDto dto);
    Task<ShopItemDto?> UpdateItemAsync(int id, SaveShopItemDto dto);
    Task<UserInventoryDto> PurchaseAsync(int itemId, string userId);
    Task<List<UserInventoryDto>> GetInventoryAsync(string userId);
    Task<bool> SetEquippedAsync(int inventoryId, bool isEquipped, string userId);
    Task<StreakDto?> GetStreakAsync(string userId);
}
