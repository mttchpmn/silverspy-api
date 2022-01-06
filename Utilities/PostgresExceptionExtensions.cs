using Npgsql;

namespace Utilities
{
    public static class PostgresExceptionExtensions
    {
        public static bool IsDuplicateException(this PostgresException exception)
        {
            var duplicateState = "23505";
            return exception.SqlState.Equals(duplicateState);
        }

        public static bool IsDatabaseExistsException(this PostgresException exception)
        {
            var databaseExistsState = "42P04";
            return exception.SqlState.Equals(databaseExistsState);
        }
        
        public static bool IsMissingForeignKeyException(this PostgresException exception)
        {
            var databaseExistsState = "23503";
            return exception.SqlState.Equals(databaseExistsState);
        }
    }
}