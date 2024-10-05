using Microsoft.EntityFrameworkCore;
using TodoAPI.API.Extensions;
using TodoAPI.API.Repositories;
using TodoAPI.Data.Models;

namespace TodoAPI.API.Services;

public class TodoTaskService : GenericService<TodoTask, int>, ITodoTaskService
{
	readonly TodoDBContext _dbContext;
	readonly ITodoTaskRepository _repository;

	readonly ITodoGoalService _goalService;
	readonly ITodoTaskGoalService _taskGoalService;

	public TodoTaskService(TodoDBContext dBContext,
		ITodoTaskRepository repository,
		ITodoGoalService goalService, ITodoTaskGoalService taskGoalService)
		: base(repository)
	{
		_dbContext = dBContext;
		_repository = repository;

		_goalService = goalService;
		_taskGoalService = taskGoalService;
	}


	#region Get
	public IQueryable<TodoTask> GetAllByGoal(int goalID, int limit = 0)
		=> _taskGoalService.GetTasksByGoalID(goalID, limit);

	public IQueryable<TodoTask> GetPendings(int limit = 0)
		=> _repository.GetAll()
			.Where((t) => t.IsCompleted == false)
			.OrderBy(t => t.ID)
			.TakeLimit(limit);


	public IQueryable<TodoTask> GetCompleteds(int limit = 0)
			=> _repository.GetAll()
			.Where((t) => t.IsCompleted == true)
			.OrderBy(t => t.ID)
			.TakeLimit(limit);

	#endregion


	#region Update
	public override async Task<TodoTask?> Update(TodoTask task)
	{
		TodoTask? updatedTask = await base.Update(task);
		if (updatedTask == null)
			return null;

		// every a task is updated, recalculate goals completed status
		bool successUpdatedStatus = await _goalService.UpdateAllCompletedStatusByTask(updatedTask.ID);
		if (!successUpdatedStatus)
			return null;

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
	public async Task<bool> DissociateGoalsByTask(int taskID)
	{
		// get goals associated with this task
		List<TodoGoal> goals = await _taskGoalService.GetGoalsByTaskID(taskID).ToListAsync();

		// Remove associations
		foreach (var g in goals)
		{
			bool success = await _taskGoalService.Dissociate(taskID, g.ID);
			if (!success)
				return false;
		}

		// recalculate goals status
		foreach (var g in goals)
		{
			bool success = await _goalService.UpdateCompletedStatus(g.ID);
			if (!success)
				return false;
		}

		return true;
	}


	public override async Task<bool> HardDelete(int id)
	{
		bool success = await DissociateGoalsByTask(id);
		if (!success)
			return false;

		// delete
		return await base.HardDelete(id);
	}

	public override async Task<bool> SoftDelete(int id)
	{
		bool success = await DissociateGoalsByTask(id);
		if (!success)
			return false;

		// delete
		return await base.SoftDelete(id);
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
