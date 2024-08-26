namespace TodoAPI.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoAPI.Models;

public class PostgreDBService : DbContext, ITodoDBHandler
{
    DbSet<TodoTask> Tasks { get; set; }

    public PostgreDBService(DbContextOptions<PostgreDBService> options) : base(options)
    {

    }
    public async Task<TodoTask?> GetTask(int id)
    {
        return await Tasks.SingleOrDefaultAsync(t => t.ID == id);

        /* SQL queries to use without an ORM 
        await using var cmd = dataSource.CreateCommand(
            $"SELECT * FROM \"Tasks\"" +
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
        return Tasks.OrderBy(t => t.ID).Take(limit).ToList();
    }

    public async Task<List<TodoTask>> GetPendingTasks(int limit = 50)
    {
        return Tasks.Where((t) => t.Completed == false).OrderBy(t => t.ID).Take(limit).ToList();

        /* SQL queries to use without an ORM 
        await using var cmd = dataSource.CreateCommand(
            $"SELECT * FROM \"Tasks\"" +
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
        return Tasks.Where((t) => t.Completed == true).OrderBy(t => t.ID).Take(limit).ToList();

        /* SQL queries to use without an ORM 
        await using var cmd = dataSource.CreateCommand(
            $"SELECT * FROM \"Tasks\"" +
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




    public async Task<TodoTask?> CreateTask(TodoTask task)
    {
        TodoTask newTask = (TodoTask)task.Clone();
        EntityEntry<TodoTask> entry = Tasks.Add(newTask);
        await SaveChangesAsync();
        return entry.Entity;

        /* SQL queries to use without an ORM 
        await using var cmd = dataSource.CreateCommand(
            $"INSERT INTO \"Tasks\"" +
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

    public async Task<bool> DeleteTask(int id)
    {
        TodoTask? task = await GetTask(id);
        if (task == null)
            return false;

        Tasks.Remove(task);
        return await SaveChangesAsync() > 0;

        /* SQL queries to use without an ORM 
        await using var cmd = dataSource.CreateCommand(
            $"DELETE FROM \"Tasks\"" +
            $"where id = '{id}';"
        );
        int affectedRows = await cmd.ExecuteNonQueryAsync();
        return affectedRows > 0;
        */
    }



    public async Task<TodoTask?> UpdateTask(TodoTask task)
    {
        TodoTask? currentTask = await GetTask(task.ID);
        if (currentTask == null)
            return null;

        Entry(currentTask).CurrentValues.SetValues(task);
        await SaveChangesAsync();
        return currentTask;

        /* SQL queries to use without an ORM 
        await using var cmd = dataSource.CreateCommand(
            $"UPDATE \"Tasks\"" +
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
        await SaveChangesAsync();
        return task;

        /* SQL queries to use without an ORM 
        await using var cmd = dataSource.CreateCommand(
        $"UPDATE \"Tasks\"" +
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
}
