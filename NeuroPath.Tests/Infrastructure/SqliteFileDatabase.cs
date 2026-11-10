using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NeuroPath.Data.Context;

namespace NeuroPath.Tests.Infrastructure;

public sealed class SqliteFileDatabase : IDisposable
{
    private readonly string _path = Path.Combine(Path.GetTempPath(), $"neuropath-tests-{Guid.NewGuid():N}.db");

    public string ConnectionString => $"Data Source={_path};Default Timeout=30;Pooling=False";

    public NeuroPathDbContext NewContext()
        => new(new DbContextOptionsBuilder<NeuroPathDbContext>().UseSqlite(ConnectionString).Options);

    public void EnsureCreated()
    {
        using var context = NewContext();
        context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        SqliteConnection.ClearAllPools();
        foreach (var file in new[] { _path, $"{_path}-journal", $"{_path}-wal", $"{_path}-shm" })
        {
            try
            {
                if (File.Exists(file)) File.Delete(file);
            }
            catch (IOException)
            {
            }
        }
    }
}
