using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace TodoAPI.API.Interceptors;

public class ReadExampleInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        Console.WriteLine("=========== Reader START ============");
        return result;

    }

    public override DbDataReader ReaderExecuted(DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
    {
        Console.WriteLine("=========== Reader END ============");
        return result;
    }


}
