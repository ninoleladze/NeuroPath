namespace NeuroPath.Core.Common;

public static class AppRoles
{
    public const string Admin = "Admin";
    public const string Trainer = "Trainer";
    public const string User = "User";
    public const string ContentManagers = Admin + "," + Trainer;

    public static readonly string[] All = { Admin, Trainer, User };
}
