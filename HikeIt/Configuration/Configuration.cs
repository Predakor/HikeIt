namespace HikeIt.Api.Configuration;

public interface IConfiguration<in T> {
    public void Apply(T config);
}

public class SqlConfiguration(string connectionString, string container)
    : IConfiguration<SqlConfiguration> {
    public string ConnectionString { get; } = connectionString;
    public string Container { get; } = container;

    public void Apply(SqlConfiguration config) {
        throw new NotImplementedException();
    }
}
