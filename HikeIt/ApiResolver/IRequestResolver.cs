namespace HikeIt.Api.ApiResolver;

public interface IRequestResolver<TResponse> {
    public TResponse Resolve<T>(T? result);
}
