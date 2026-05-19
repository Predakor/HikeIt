using Microsoft.Extensions.DependencyInjection;

namespace Application.Commons.Abstractions.Queries;

public interface IQueryDispatcher
{
    Task<Result<TResponse>> Send<TQuery, TResponse>(
        TQuery query,
        CancellationToken cancellationToken
    )
        where TQuery : IQuery<TResponse>;
}

internal sealed class QueryDispatcher : IQueryDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public QueryDispatcher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task<Result<TResponse>> Send<TQuery, TResponse>(
        TQuery query,
        CancellationToken cancellationToken
    )
        where TQuery : IQuery<TResponse>
    {
        var handler = _serviceProvider.GetRequiredService<IQueryHandler<TQuery, TResponse>>();
        return handler.Handle(query, cancellationToken);
    }
}