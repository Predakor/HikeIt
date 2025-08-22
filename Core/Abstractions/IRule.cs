namespace Core.Abstractions;

public interface IRuleBase {
    string Name { get; }
    string Message { get; }
}

public interface IRule : IRuleBase {
    Result<bool> Check();
}

public interface IRuleAsync : IRuleBase {
    Task<Result<bool>> CheckAsync();
}
