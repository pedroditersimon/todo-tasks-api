﻿namespace TodoAPI.Models;

public class TodoGoal
{
    public int ID { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }

    public ICollection<TodoTask> Tasks { get; set; }
}
