namespace TodoAPI.Interceptors;

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Data.Common;

public class SecondLevelCacheInterceptor(IMemoryCache cache) : DbCommandInterceptor
{

    public override async ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command,
        CommandEventData eventData, InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        string key = command.CommandText +
                     string.Join(",", command.Parameters.Cast<DbParameter>().Select(p => p.Value));

        if (cache.TryGetValue(key, out List<Dictionary<string, object>>? cacheEntry))
        {
            Console.WriteLine("==== READ FROm CACHE ===");
            var reader = CreateDataReaderFromCacheEntry(cacheEntry);
            return InterceptionResult<DbDataReader>.SuppressWithResult(reader);
        }

        return result;
    }

    public override async ValueTask<DbDataReader> ReaderExecutedAsync(DbCommand command,
        CommandExecutedEventData eventData, DbDataReader result, CancellationToken cancellationToken = default)
    {
        var key = command.CommandText + string.Join(",", command.Parameters.Cast<DbParameter>().Select(p => p.Value));


        var resultsList = new List<Dictionary<string, object>>();
        if (result.HasRows)
        {
            while (await result.ReadAsync(cancellationToken))
            {
                var row = new Dictionary<string, object>();
                for (var i = 0; i < result.FieldCount; i++)
                {
                    row.TryAdd(result.GetName(i), result.GetValue(i));
                }

                resultsList.Add(row);
            }

            if (resultsList.Count != 0)
            {
                cache.Set(key, resultsList);
            }
        }

        result.Close();

        return CreateDataReaderFromCacheEntry(resultsList);
    }

    DataTableReader CreateDataReaderFromCacheEntry(List<Dictionary<string, object>>? cacheEntry)
    {
        var table = new DataTable();
        if (cacheEntry != null && cacheEntry.Count != 0)
        {
            foreach (var pair in cacheEntry.First())
            {
                table.Columns.Add(pair.Key,
                    pair.Value is not null && pair.Value?.GetType() != typeof(DBNull)
                        ? pair.Value.GetType()
                        : typeof(object));
            }

            foreach (var row in cacheEntry)
            {
                table.Rows.Add(row.Values.ToArray());
            }
        }

        return table.CreateDataReader();
    }
}
