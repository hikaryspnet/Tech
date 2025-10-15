using Tech.Core.Auth.Common.Result;
using System.Data;

namespace Tech.Core.Transactions
{
    public interface ITransactionalExecutor
    {
        /// <summary>
        /// Виконує вказану дію всередені транзакції з заданим рівнем ізоляції.
        /// </summary>
        /// <param name="action">Асинхнронна дія, яка виконується всередині транзакції.</param>
        /// <param name="isolationLevel">Рівнь ізоляції транзакції.</param>
        /// <param name="cancellationToken">Токен відміни</param>
        Task StartEffect(Func<CancellationToken, Task> action, IsolationLevel isolationLevel, CancellationToken cancellationToken = default);
        Task<Result<TResult>> StartEffect<TResult>(Func<CancellationToken, Task<Result<TResult>>> action, IsolationLevel isolationLevel, CancellationToken cancellationToken = default);
    }
}
