namespace NeuroPath.Core.Models.Domain.Admin;

public class FaqCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public ICollection<Faq> Faqs { get; set; } = new List<Faq>();
}
