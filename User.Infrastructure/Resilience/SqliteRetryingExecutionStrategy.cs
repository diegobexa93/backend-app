using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Storage;

namespace User.Infrastructure.Resilience
{
    public class SqliteRetryingExecutionStrategy : ExecutionStrategy
    {
        public SqliteRetryingExecutionStrategy(
            ExecutionStrategyDependencies dependencies,
            int maxRetryCount = 5,
            TimeSpan maxRetryDelay = default)
            : base(dependencies, maxRetryCount, maxRetryDelay)
        {
        }

        protected override bool ShouldRetryOn(Exception exception)
        {
            // Add conditions for which exceptions should trigger a retry
            if (exception is SqliteException sqliteException)
            {
                // Example: Retry on database lock or busy state
                return sqliteException.SqliteErrorCode == SQLitePCL.raw.SQLITE_BUSY ||
                       sqliteException.SqliteErrorCode == SQLitePCL.raw.SQLITE_LOCKED;
            }

            return false;
        }
    }
}
