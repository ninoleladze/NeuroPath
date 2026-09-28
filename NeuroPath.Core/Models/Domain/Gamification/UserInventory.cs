using NeuroPath.Core.Models.Domain.Identity;

namespace NeuroPath.Core.Models.Domain.Gamification;

public class UserInventory
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int ShopItemId { get; set; }
    public DateTime PurchasedAt { get; set; } = DateTime.UtcNow;
    public bool IsEquipped { get; set; } = false;
    public User User { get; set; } = null!;
    public ShopItem ShopItem { get; set; } = null!;
}
