namespace NeuroPath.Core.Models.Domain.Gamification;

public class ShopItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Price { get; set; }
    public string? ImageUrl { get; set; }
    public string Category { get; set; } = string.Empty;
    public int? StockQuantity { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<UserInventory> UserInventories { get; set; } = new List<UserInventory>();
}
