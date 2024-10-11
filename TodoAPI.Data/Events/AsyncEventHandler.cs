namespace TodoAPI.Data.Events;


public delegate System.Threading.Tasks.Task AsyncEventHandler<T>(object sender, T args);
