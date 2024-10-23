using Microsoft.EntityFrameworkCore;
using TodoAPI.API.Extensions;
using TodoAPI.API.Repositories;
using TodoAPI.Data.Events;
using TodoAPI.Data.Events.Task;
using TodoAPI.Data.Models;

namespace TodoAPI.API.Services;

public class TodoTaskService : GenericService<TodoTask, int>, ITodoTaskService
{
	readonly TodoDBContext _dbContext;
	readonly ITodoTaskRepository _repository;

	readonly ITodoTaskGoalService _taskGoalService;

	// Events
	public event AsyncEventHandler<TaskIsDeletedEventArgs> OnTaskIsDeleted;
	public event AsyncEventHandler<TaskIsUpdatedEventArgs> OnTaskIsUpdated;


	public TodoTaskService(TodoDBContext dBContext,
		ITodoTaskRepository repository, ITodoTaskGoalService taskGoalService)
		: base(repository)
	{
		_dbContext = dBContext;
		_repository = repository;

		_taskGoalService = taskGoalService;
	}


	#region Get
	public IQueryable<TodoTask> GetAllByGoal(int goalID, int limit = 0)
		=> _taskGoalService.GetTasksByGoalID(goalID, limit);

	public IQueryable<TodoTask> GetPendings(int limit = 0)
		=> _repository.GetAll()
			.Where((t) => t.IsCompleted == false)
			.TakeLimit(limit);


	public IQueryable<TodoTask> GetCompleteds(int limit = 0)
			=> _repository.GetAll()
			.Where((t) => t.IsCompleted == true)
			.TakeLimit(limit);

	#endregion


	#region Update
	public override async Task<TodoTask?> Update(TodoTask task)
	{
		TodoTask? updatedTask = await base.Update(task);
		if (updatedTask == null)
			return null;

		await OnTaskIsUpdated(this, new TaskIsUpdatedEventArgs(task.ID, task.IsCompleted));
		return updatedTask;
	}

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


	#region Delete

	public override async Task<bool> HardDelete(int id)
	{
		// delete
		bool success = await base.HardDelete(id);
		if (!success)
			return false;

		await OnTaskIsDeleted(this, new TaskIsDeletedEventArgs(id));
		return true;
	}

	public override async Task<bool> SoftDelete(int id)
	{
		/*
		bool success = await DissociateGoalsByTask(id);
		if (!success)
			return false;
		*/

		// delete
		bool success = await base.SoftDelete(id);
		if (!success)
			return false;

		await OnTaskIsDeleted(this, new TaskIsDeletedEventArgs(id));
		return true;
	}
	#endregion


	#region RawSQL Test
	public async Task<TodoTask?> RawSQL_GetById(int id)
		=> await _dbContext.Database
			.SqlQueryRaw<TodoTask>("SELECT * FROM \"TodoTask\" WHERE \"ID\" = {0}", id)
			.FirstOrDefaultAsync();

	public async Task<TodoTask?> RawSQLWithDBSet_GetById(int id)
	   => await _dbContext.Set<TodoTask>()
			.FromSqlInterpolated($"SELECT * FROM \"TodoTask\" WHERE \"ID\" = {id}")
			.FirstOrDefaultAsync();
	#endregion

	#region Stored Procedures
	public async Task<TodoTask?> StoredProcedure_GetByID(int id)
	   => await _dbContext.Set<TodoTask>()
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
