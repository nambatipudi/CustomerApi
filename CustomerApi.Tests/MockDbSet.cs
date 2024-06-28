using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CustomerApi.Tests;

public class MockDbSet<T> : DbSet<T>, IAsyncEnumerable<T>, IQueryable<T> where T : class
{
    private readonly IQueryable<T> _queryable;

    public MockDbSet(IQueryable<T> queryable)
    {
        _queryable = queryable;
    }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new AsyncEnumerator<T>(_queryable.GetEnumerator());
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return _queryable.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _queryable.GetEnumerator();
    }

    Type IQueryable.ElementType => _queryable.ElementType;

    Expression IQueryable.Expression => _queryable.Expression;

    IQueryProvider IQueryable.Provider => _queryable.Provider;

    public override IEntityType EntityType => throw new NotImplementedException();
}

