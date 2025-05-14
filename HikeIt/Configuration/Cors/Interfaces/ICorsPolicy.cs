namespace HikeIt.Api.Configuration.Cors.Interfaces;

public interface ICorsPolicy {
    void ApplyCorsPolicy(IServiceCollection services);
}
