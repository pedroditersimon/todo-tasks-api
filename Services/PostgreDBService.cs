namespace TodoAPI.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoAPI.Migrations.Seeds;
using TodoAPI.Models;
using TodoAPI.Repositories;

public class PostgreDBService : DbContext, ITodoTaskRepository, ITodoGoalRepository
{
    DbSet<TodoTask> Tasks { get; set; }
    DbSet<TodoGoal> Goals { get; set; }

    public PostgreDBService(DbContextOptions<PostgreDBService> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        /*
        // Create database with this initial values (seed)
        modelBuilder.Entity<TodoTask>().HasData(
            new TodoTask() { ID = 1, Name = "my First Task", Description = "Inserted by ef migrations" },
            new TodoTask() { ID = 2, Name = "TodoTask1", Description = "ModelBuilder builder" }
        );
        */

        /*
        // Handling seeds with a class (constructor)
        new TodoTaskSeed(modelBuilder);
        */

        // Handling seeds with IEntityTypeConfiguration
        modelBuilder.ApplyConfiguration(new TodoTaskSeed());
    }


    #region Task

    // Get
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

    // Create
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

    // Delete
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

    // Update
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
    public async Task<TodoTask?> SetTaskGoal(int taskID, int goalID)
    {
        // task need to exists
        TodoTask? task = await GetTask(taskID);
        if (task == null)
            return null;

        // goal need to exists
        TodoGoal? goal = await GetGoal(goalID);
        if (goal == null)
            return null;

        /* ef automatically does the check
        // get the current goal of task
        Goal? currentGoal = Goals.Include(g => g.Tasks)
            .SingleOrDefault(g => g.Tasks.Any(t => t.ID == taskID));

        // already in
        if (currentGoal == goal)
            return task;

        // remove task from current goal
        currentGoal?.Tasks.Remove(task);
        */

        // add task to new goal
        goal.Tasks.Add(task);

        await SaveChangesAsync();

        return task;
    }
    #endregion

    #region Goal

    // Get
    public async Task<TodoGoal?> GetGoal(int id, bool includeTasks = true)
    {
        // include tasks
        IQueryable<TodoGoal> query = includeTasks ? Goals.Include(g => g.Tasks) : Goals;

        return await query.SingleOrDefaultAsync(g => g.ID == id);
    }

    public async Task<List<TodoGoal>> GetAllGoals(int limit = 50, bool includeTasks = true)
    {
        // include tasks
        IQueryable<TodoGoal> query = includeTasks ? Goals.Include(g => g.Tasks) : Goals;

        return query.OrderBy(g => g.ID).Take(limit).ToList();
    }

    public async Task<List<TodoGoal>> GetPendingGoals(int limit = 50, bool includeTasks = true)
    {
        // include tasks
        IQueryable<TodoGoal> query = includeTasks ? Goals.Include(g => g.Tasks) : Goals;

        return query.Where((g) => g.Tasks.Any(task => !task.Completed)).OrderBy(g => g.ID).Take(limit).ToList();
    }

    public async Task<List<TodoGoal>> GetCompletedGoals(int limit = 50, bool includeTasks = true)
    {
        // include tasks
        IQueryable<TodoGoal> query = includeTasks ? Goals.Include(g => g.Tasks) : Goals;

        return query.Where((g) => !g.Tasks.Any(task => !task.Completed)).OrderBy(g => g.ID).Take(limit).ToList();
    }

    // Create
    public async Task<TodoGoal?> CreateGoal(TodoGoal goal)
    {
        TodoGoal newGoal = (TodoGoal)goal.Clone();
        EntityEntry<TodoGoal> entry = Goals.Add(newGoal);
        await SaveChangesAsync();
        return entry.Entity;
    }

    // Delete
    public async Task<bool> DeleteGoal(int id)
    {
        TodoGoal? goal = await GetGoal(id);
        if (goal == null)
            return false;

        Tasks.RemoveRange(goal.Tasks);
        Goals.Remove(goal);
        return await SaveChangesAsync() > 0;
    }

    // Update
    public async Task<TodoGoal?> UpdateGoal(TodoGoal goal)
    {
        TodoGoal? currentGoal = await GetGoal(goal.ID);
        if (currentGoal == null)
            return null;

        Entry(currentGoal).CurrentValues.SetValues(goal);
        await SaveChangesAsync();
        return currentGoal;
    }

    #endregion
}
