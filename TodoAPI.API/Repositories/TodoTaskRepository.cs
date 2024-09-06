using Microsoft.EntityFrameworkCore;
using TodoAPI.API.Models;
using TodoAPI.API.Services;

namespace TodoAPI.API.Repositories;

public class TodoTaskRepository(TodoDBContext dbContext)
    : GenericRepository<TodoTask, int>(dbContext), ITodoTaskRepository
{

    #region Get
    public IQueryable<TodoTask> GetPendings(int limit = 50)
        => Entities
            .Where((t) => t.IsCompleted == false)
            .OrderBy(t => t.ID)
            .Take(limit);


    public IQueryable<TodoTask> GetCompleteds(int limit = 50)
            => Entities
            .Where((t) => t.IsCompleted == true)
            .OrderBy(t => t.ID)
            .Take(limit);

    #endregion


    #region Update
    public async Task<TodoTask?> SetCompleted(int id, bool completed)
    {
        TodoTask? task = await GetByID(id);
        if (task == null)
            return null;

        task.IsCompleted = completed;
        return await Update(task);

        /* Raw sql
        await using var cmd = dataSource.CreateCommand(
        $"UPDATE \"dbContext.Tasks\"" +
        $"SET completed='{completed}'" +
        $"where id = '{id}'" +
        $"RETURNING id, name, description, completed;"
        );
        await using var reader = await cmd.ExecuteReaderAsync();

        if (!reader.HasRows)
            return null;

        // read first row
        await reader.ReadAsync();

        return new TodoTask()
        {
            ID = reader.GetFieldValue<int>(0),
            Name = reader.GetFieldValue<string>(1),
            Description = reader.GetFieldValue<string>(2),
            Completed = reader.GetFieldValue<bool>(3)
        };
        */
    }
    #endregion

    #region RawSQL Test
    public async Task<TodoTask?> RawSQL_GetById(int id)
        => await dbContext.Database
            .SqlQueryRaw<TodoTask>("SELECT * FROM \"TodoTask\" WHERE \"ID\" = {0}", id)
            .FirstOrDefaultAsync();

    public async Task<TodoTask?> RawSQLWithDBSet_GetById(int id)
       => await Entities
            .FromSqlInterpolated($"SELECT * FROM \"TodoTask\" WHERE \"ID\" = {id}")
            .FirstOrDefaultAsync();
    #endregion

    #region Stored Procedures
    public async Task<TodoTask?> StoredProcedure_GetByID(int id)
       => await Entities
            .FromSqlInterpolated($"SELECT * FROM GetTaskByID({id})")
            .FirstOrDefaultAsync();

    /* Create function script (postgresql)
    --DROP FUNCTION gettaskbyid(integer);

    CREATE OR REPLACE FUNCTION GetTaskByID(tID integer)
    RETURNS TABLE (
        "ID" integer, 
        "Name" text, 
        "Description" text, 
        "IsCompleted" boolean, 
        "CreationDate" timestamp with time zone, 
        "IsDeleted" boolean, 
        "LastUpdatedTime" timestamp with time zone,
	    "TodoGoalID" integer
    ) AS $$
    BEGIN
    RETURN QUERY 
        SELECT 
            t."ID", 
            t."Name", 
            t."Description", 
            t."IsCompleted", 
            t."CreationDate", 
            t."IsDeleted", 
            t."LastUpdatedTime",
		    t."TodoGoalID"
        FROM public."TodoTask" t
        WHERE t."ID" = tID;
    END;
    $$ LANGUAGE plpgsql;
     */
    #endregion
}
