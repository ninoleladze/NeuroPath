namespace NeuroPath.Core.Common;

public static class Paging
{
    public const int DefaultPageSize = 20;
    public const int MaxPageSize = 100;

    public static (int Skip, int Take) Normalize(int page, int pageSize)
    {
        var safePage = page < 1 ? 1 : page;
        var safeSize = pageSize < 1 ? DefaultPageSize : Math.Min(pageSize, MaxPageSize);
        return ((safePage - 1) * safeSize, safeSize);
    }
}
