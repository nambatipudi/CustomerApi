using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace CustomerApi.Tests;
public class AsyncQueryProvider : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    public AsyncQueryProvider(IQueryProvider inner)
    {
        _inner = inner;
    }

    public IQueryable CreateQuery(Expression expression)
    {
        Type elementType = expression.Type.GetGenericArguments().First();
        try
        {
            return (IQueryable)Activator.CreateInstance(typeof(AsyncQueryable<>).MakeGenericType(elementType), new object[] { this, expression });
        }
        catch (TargetInvocationException tie)
        {
            throw tie.InnerException;
        }
    }
    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return new AsyncQueryable<TElement>(this, expression);
    }

    public object Execute(Expression expression)
    {
        return _inner.Execute(expression);
    }

    public TResult Execute<TResult>(Expression expression)
    {
        return _inner.Execute<TResult>(expression);
    }

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
    {
        return _inner.Execute<TResult>(expression);
    }

    public IAsyncEnumerator<T> GetAsyncEnumerator<T>(IQueryable<T> source, CancellationToken cancellationToken = default)
    {
        return new AsyncEnumerator<T>(source.GetEnumerator());
    }
}