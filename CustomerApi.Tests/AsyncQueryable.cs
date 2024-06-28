using System.Collections;
using System.Linq.Expressions;

namespace CustomerApi.Tests;

public class AsyncQueryable<T> : IQueryable<T>, IAsyncEnumerable<T>
{
    private readonly AsyncQueryProvider _provider;
    private readonly Expression _expression;

    public AsyncQueryable(AsyncQueryProvider provider, Expression expression)
    {
        _provider = provider;
        _expression = expression;
    }

    public Type ElementType => typeof(T);

    public Expression Expression => _expression;

    public IQueryProvider Provider => _provider;

    public IEnumerator<T> GetEnumerator()
    {
        return ((IQueryable<T>)this).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return (IAsyncEnumerator<T>)_provider.ExecuteAsync<T>(_expression, cancellationToken);
    }
}
