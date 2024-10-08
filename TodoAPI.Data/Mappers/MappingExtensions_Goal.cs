using AutoMapper;
using TodoAPI.Data.DTOs.TodoGoal;
using TodoAPI.Data.Models;

namespace TodoAPI.Data.Mappers;

public static partial class MappingExtensions
{

	// CreateGoalRequest -> TodoGoal
	public static TodoGoal ToGoal(this CreateGoalRequest createGoalRequest, IMapper mapper)
		=> mapper.Map<TodoGoal>(createGoalRequest);

	// UpdateGoalRequest -> TodoGoal
	public static TodoGoal ToGoal(this UpdateGoalRequest updateGoalRequest, IMapper mapper)
		=> mapper.Map<TodoGoal>(updateGoalRequest);


	// TodoGoal -> GoalResponse
	public static GoalResponse ToResponse(this TodoGoal goal, IMapper mapper)
		=> mapper.Map<GoalResponse>(goal);

}
