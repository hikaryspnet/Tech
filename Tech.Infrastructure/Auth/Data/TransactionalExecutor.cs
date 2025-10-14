using FluentResults;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Tech.Core.Auth.Common;


namespace Tech.Core.Transactions
{
    public class TransactionalExecutor<TContext> : ITransactionalExecutor
    where TContext : DbContext
    {
        private readonly TContext _dbContext;

        public TransactionalExecutor(TContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Result<TResult>> StartEffect<TResult>(Func<CancellationToken, Task<Result<TResult>>> action, IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(action);

            if (_dbContext.Database.CurrentTransaction is not null)
                return await action(cancellationToken);

            var strategy = _dbContext.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
                try
                {
                    var result = await action(cancellationToken);
                    if (result.IsSuccess)
                    {
                        await transaction.CommitAsync(cancellationToken);
                    }
                    else
                    {
                        await transaction.RollbackAsync(cancellationToken);
                    }
                    return result;
                }
                catch(Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return Result.Fail($"Транзакційна помилка: {ex.Message}");
                }
            });
        }

        public async Task StartEffect(Func<CancellationToken, Task> action, IsolationLevel isolationLevel, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(action);

            if (_dbContext.Database.CurrentTransaction is not null)
            {
                await action(cancellationToken);
                return;
            }

            var strategy = _dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
                try
                {
                    await action(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            });
        }
    }
}
