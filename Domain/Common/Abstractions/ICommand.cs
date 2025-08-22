using Domain.Common.Result;

namespace Domain.Common.Abstractions;

public interface ICommandBase { }

public interface ICommand<T> : ICommandBase {
    Result<T> Execute();
}

public interface IAsyncCommand<T> : ICommandBase {
    Task<Result<T>> Execute();
}
