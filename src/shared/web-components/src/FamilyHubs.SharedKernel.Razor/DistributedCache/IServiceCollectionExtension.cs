using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Distributed;

namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtension
{
    // passing Action<ReferralDistributedCacheOptions> would follow the standard pattern
    public static IServiceCollection AddRedisDistributedCache(
        this IServiceCollection services,
        string? connectionString,
        int slidingExpirationInMinutes,
        string instanceName)
    {
        ArgumentNullException.ThrowIfNull(connectionString);

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
            options.InstanceName = instanceName;
        });

        // there's currently only one, so this should be fine
        services.AddSingleton(new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(slidingExpirationInMinutes)
        });

        return services;
    }

    public static IServiceCollection AddSqlServerDistributedCache(
        this IServiceCollection services,
        string connectionString,
        int slidingExpirationInMinutes,
        string schemaName,
        string tableName)
    {
        EnsureSqlCacheTableExists(connectionString, schemaName, tableName);

        services.AddDistributedSqlServerCache(options =>
        {
            options.ConnectionString = connectionString;
            options.SchemaName = schemaName;
            options.TableName = tableName;
        });

        // there's currently only one, so this should be fine
        services.AddSingleton(new DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(slidingExpirationInMinutes)
        });

        return services;
    }

    /// <summary>
    /// Creates the cache table in Sql if it doesn't exist.
    /// Replicates dotnet sql-cache create
    /// </summary>
    private static void EnsureSqlCacheTableExists(string connectionString, string schemaName, string tableName)
    {
        // we call the db sync, as this will be called during startup

        string fullTableName = $"[{schemaName}].[{tableName}]";

        using var sqlConnection = new SqlConnection(connectionString);
        sqlConnection.Open();

        var checkTableExistsCommandText = $"IF EXISTS(SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='{schemaName}' AND TABLE_NAME='{tableName}') SELECT 1 ELSE SELECT 0";
        var checkCmd = new SqlCommand(checkTableExistsCommandText, sqlConnection);
        
        // IF EXISTS returns the SELECT 1 if the table exists or SELECT 0 if not
        var tableExists = Convert.ToInt32(checkCmd.ExecuteScalar());
        if (tableExists == 1)
        {
            return;
        }

        var createTableExistsCommandText = @$"
CREATE TABLE {fullTableName}(Id nvarchar(449) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL, Value varbinary(MAX) NOT NULL, ExpiresAtTime datetimeoffset NOT NULL, SlidingExpirationInSeconds bigint NULL,AbsoluteExpiration datetimeoffset NULL, PRIMARY KEY (Id));
CREATE NONCLUSTERED INDEX Index_ExpiresAtTime ON {fullTableName}(ExpiresAtTime);";

        var createCmd = new SqlCommand(createTableExistsCommandText, sqlConnection);
        createCmd.ExecuteNonQuery();
        sqlConnection.Close();
    }
}