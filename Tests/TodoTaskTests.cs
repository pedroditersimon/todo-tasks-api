using Microsoft.EntityFrameworkCore;
using TodoAPI.Models;
using TodoAPI.Repositories;
using TodoAPI.Services;
using Xunit;

namespace TodoAPI.Tests;
public class TodoTaskTests
{

    static TodoDBContext CreateDBContext()
    {
        DbContextOptions<TodoDBContext> options = new DbContextOptionsBuilder<TodoDBContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new TodoDBContext(options);
    }


    [Fact]
    public async void GetByID()
    {
        using TodoDBContext dbContext = CreateDBContext();
        TodoTaskRepository taskRepository = new(dbContext);

        await taskRepository.Create(new TodoTask() { ID = 1 });
        await dbContext.SaveChangesAsync();

        TodoTask? task = await taskRepository.GetByID(1);
        Assert.True(task != null);
    }


    [Fact]
    public async void GetByID_NotFound()
    {
        using TodoDBContext dbContext = CreateDBContext();
        TodoTaskRepository taskRepository = new(dbContext);

        await taskRepository.Create(new TodoTask() { ID = 1 });
        await dbContext.SaveChangesAsync();

        TodoTask? task = await taskRepository.GetByID(2);
        Assert.True(task == null);
    }
}