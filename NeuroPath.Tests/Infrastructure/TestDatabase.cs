using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NeuroPath.Core.Interfaces;
using NeuroPath.Core.Services;
using NeuroPath.Data.Context;

namespace NeuroPath.Tests.Infrastructure;

public sealed class TestDatabase : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<NeuroPathDbContext> _options;

    public TestDatabase()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        _options = new DbContextOptionsBuilder<NeuroPathDbContext>().UseSqlite(_connection).Options;
        Context = new NeuroPathDbContext(_options);
        Context.Database.EnsureCreated();
        Cache = new CacheService(new MemoryCache(new MemoryCacheOptions()));
    }

    public NeuroPathDbContext Context { get; }

    public ICacheService Cache { get; }

    public NeuroPathDbContext NewContext() => new(_options);

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
    }
}
