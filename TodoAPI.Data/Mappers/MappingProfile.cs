using AutoMapper;
using TodoAPI.Data.DTOs.TodoTask;
using TodoAPI.Data.Models;

namespace TodoAPI.Data.Mappers;

public class MappingProfile : Profile
{
	// Constructor donde definimos los mapeos
	public MappingProfile()
	{
		CreateMap<CreateTaskRequest, TodoTask>();
		CreateMap<TodoTask, TaskResponse>();
	}
}
