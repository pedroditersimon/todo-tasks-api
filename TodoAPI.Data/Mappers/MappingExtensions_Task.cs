using AutoMapper;
using TodoAPI.Data.DTOs.TodoTask;
using TodoAPI.Data.Models;

namespace TodoAPI.Data.Mappers;

public static partial class MappingExtensions
{

	// CreateTaskRequest -> TodoTask
	public static TodoTask ToTask(this CreateTaskRequest createTaskRequest, IMapper mapper)
		=> mapper.Map<TodoTask>(createTaskRequest);

	// UpdateTaskRequest -> TodoTask
	public static TodoTask ToTask(this UpdateTaskRequest updateTaskRequest, IMapper mapper)
		=> mapper.Map<TodoTask>(updateTaskRequest);


	// TodoTask -> TaskResponse
	public static TaskResponse ToResponse(this TodoTask task, IMapper mapper)
		=> mapper.Map<TaskResponse>(task);

}
