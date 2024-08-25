namespace TodoAPI.Services;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoAPI.Models;

public class PostgreDBService : ITodoDBHandler
{
    readonly NpgsqlDataSource dataSource;

    public PostgreDBService(string host, string username, string password, string databaseName)
    {
        var connectionString = $"Host={host};Username={username};Password={password};Database={databaseName}";
        dataSource = NpgsqlDataSource.Create(connectionString);
    }


    public async Task<TodoTask?> CreateTask(TodoTask task)
    {
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
    }

    public async Task<bool> DeleteTask(int id)
    {
        await using var cmd = dataSource.CreateCommand(
            $"DELETE FROM \"Tasks\"" +
            $"where id = '{id}';"
        );
        int affectedRows = await cmd.ExecuteNonQueryAsync();
        return affectedRows > 0;
    }

    public async Task<List<TodoTask>> GetCompletedTasks()
    {
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
    }

    public async Task<List<TodoTask>> GetPendingTasks()
    {
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
    }

    public async Task<TodoTask?> GetTask(int id)
    {
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
    }

    public async Task<TodoTask?> SetCompletedTask(int id, bool completed)
    {
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
    }

    public async Task<TodoTask?> UpdateTask(TodoTask task)
    {
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
    }
}
