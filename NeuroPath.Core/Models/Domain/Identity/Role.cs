using Microsoft.AspNetCore.Identity;

namespace NeuroPath.Core.Models.Domain.Identity;

public class Role : IdentityRole
{
    public string? Description { get; set; }
}
