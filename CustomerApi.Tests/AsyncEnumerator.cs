namespace CustomerApi.Tests;

public class AsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> enumeratorField;

    public AsyncEnumerator(IEnumerator<T> enumerator)
    {
        enumeratorField = enumerator;
    }

    public T Current => enumeratorField.Current;

    public ValueTask<bool> MoveNextAsync(CancellationToken cancellationToken)
    {
        return new ValueTask<bool>(enumeratorField.MoveNext());
    }

    public ValueTask DisposeAsync()
    {
        enumeratorField.Dispose();
        return new ValueTask();
    }

    public ValueTask<bool> MoveNextAsync()
    {
        return new ValueTask<bool>(enumeratorField.MoveNext());
    }
}
