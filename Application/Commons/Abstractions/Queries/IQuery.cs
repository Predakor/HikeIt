namespace Application.Commons.Abstractions.Queries;

public interface IQuery<TResponse> { }

public interface IQueryHandler<in TQuery, TResponse>
    where TQuery : IQuery<TResponse> {
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
}
