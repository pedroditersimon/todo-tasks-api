using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TodoAPI.Models;
using TodoAPI.Services;

namespace TodoAPI.Repositories;

public class TodoTaskRepository(TodoDBContext dbContext) : ITodoTaskRepository
{



    // Get
    public async Task<TodoTask?> GetTask(int id)
    {
        return await dbContext.Tasks.SingleOrDefaultAsync(t => t.ID == id);

        /* Raw sql
        await using var cmd = dataSource.CreateCommand(
            $"SELECT * FROM \"dbContext.Tasks\"" +
            $"where id = {id};"
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

    public async Task<List<TodoTask>> GetAllTask(int limit = 50)
    {
        return dbContext.Tasks.OrderBy(t => t.ID).Take(limit).ToList();
    }

    public async Task<List<TodoTask>> GetPendingTasks(int limit = 50)
    {
        return dbContext.Tasks.Where((t) => t.Completed == false).OrderBy(t => t.ID).Take(limit).ToList();

        /* Raw sql
        await using var cmd = dataSource.CreateCommand(
            $"SELECT * FROM \"dbContext.Tasks\"" +
            $"where completed = false;"
        );
        await using var reader = await cmd.ExecuteReaderAsync();

        List<TodoTask> tasks = new();

        // return empty
        if (!reader.HasRows)
            return tasks;

        // read first row
        while (await reader.ReadAsync())
        {
            TodoTask t = new()
            {
                ID = reader.GetFieldValue<int>(0),
                Name = reader.GetFieldValue<string>(1),
                Description = reader.GetFieldValue<string>(2),
                Completed = reader.GetFieldValue<bool>(3)
            };
            tasks.Add(t);
        }

        return tasks;
        */
    }

    public async Task<List<TodoTask>> GetCompletedTasks(int limit = 50)
    {
        return dbContext.Tasks.Where((t) => t.Completed == true).OrderBy(t => t.ID).Take(limit).ToList();

        /* Raw sql
        await using var cmd = dataSource.CreateCommand(
            $"SELECT * FROM \"dbContext.Tasks\"" +
            $"where completed = true;"
        );
        await using var reader = await cmd.ExecuteReaderAsync();

        List<TodoTask> tasks = new();

        // return empty
        if (!reader.HasRows)
            return tasks;

        // read first row
        while (await reader.ReadAsync())
        {
            TodoTask t = new()
            {
                ID = reader.GetFieldValue<int>(0),
                Name = reader.GetFieldValue<string>(1),
                Description = reader.GetFieldValue<string>(2),
                Completed = reader.GetFieldValue<bool>(3)
            };
            tasks.Add(t);
        }

        return tasks;
        */
    }

    // Create
    public async Task<TodoTask?> CreateTask(TodoTask task)
    {
        EntityEntry<TodoTask> entry = dbContext.Tasks.Add(task);
        return entry.Entity;

        /* Raw sql
        await using var cmd = dataSource.CreateCommand(
            $"INSERT INTO \"dbContext.Tasks\"" +
            $"(name, description, completed)" +
            $"VALUES ('{task.Name}', '{task.Description}', '{task.Completed}')" +
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

    // Delete
    public async Task<bool> DeleteTask(int id)
    {
        TodoTask? task = await GetTask(id);
        if (task == null)
            return false;

        //dbContext.Tasks.Remove(task);
        task.IsDeleted = true;

        return true;

        /* Raw sql
        await using var cmd = dataSource.CreateCommand(
            $"DELETE FROM \"dbContext.Tasks\"" +
            $"where id = '{id}';"
        );
        int affectedRows = await cmd.ExecuteNonQueryAsync();
        return affectedRows > 0;
        */
    }

    // Update
    public async Task<TodoTask?> UpdateTask(TodoTask task)
    {
        TodoTask? currentTask = await GetTask(task.ID);
        if (currentTask == null)
            return null;

        dbContext.Entry(currentTask).CurrentValues.SetValues(task);
        return currentTask;

        /* Raw sql
        await using var cmd = dataSource.CreateCommand(
            $"UPDATE \"dbContext.Tasks\"" +
            $"SET name='{task.Name}', description='{task.Description}', completed='{task.Completed}'" +
            $"where id = '{task.ID}'" +
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
    public async Task<TodoTask?> SetCompletedTask(int id, bool completed)
    {
        TodoTask? task = await GetTask(id);
        if (task == null)
            return null;

        task.Completed = completed;
        return task;

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
    public async Task<TodoTask?> SetTaskGoal(int taskID, int goalID)
    {
        // task need to exists
        TodoTask? task = await GetTask(taskID);
        if (task == null)
            return null;

        // goal need to exists
        TodoGoal? goal = await dbContext.Goals.Include(g => g.Tasks).SingleOrDefaultAsync(g => g.ID == goalID);
        if (goal == null)
            return null;

        /* ef automatically does the check
        // get the current goal of task
        Goal? currentGoal = Goals.Include(g => g.dbContext.Tasks)
            .SingleOrDefault(g => g.dbContext.Tasks.Any(t => t.ID == taskID));

        // already in
        if (currentGoal == goal)
            return task;

        // remove task from current goal
        currentGoal?.dbContext.Tasks.Remove(task);
        */

        // add task to new goal
        goal.Tasks.Add(task);

        return task;
    }
}
