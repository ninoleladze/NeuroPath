using NeuroPath.Core.Common;

namespace NeuroPath.Tests.Common;

public class PagingTests
{
    [Theory]
    [InlineData(1, 20, 0, 20)]
    [InlineData(3, 10, 20, 10)]
    [InlineData(0, 20, 0, 20)]
    [InlineData(-5, 20, 0, 20)]
    [InlineData(1, 0, 0, Paging.DefaultPageSize)]
    [InlineData(1, 5000, 0, Paging.MaxPageSize)]
    public void Normalize_ClampsPageAndSize(int page, int pageSize, int expectedSkip, int expectedTake)
    {
        var (skip, take) = Paging.Normalize(page, pageSize);

        Assert.Equal(expectedSkip, skip);
        Assert.Equal(expectedTake, take);
    }

    [Fact]
    public void TokenHasher_IsDeterministicAndDoesNotReturnInput()
    {
        var token = TokenHasher.NewToken();

        Assert.Equal(TokenHasher.Hash(token), TokenHasher.Hash(token));
        Assert.NotEqual(token, TokenHasher.Hash(token));
        Assert.NotEqual(TokenHasher.NewToken(), token);
    }
}
