namespace TodoAPI.API.Extensions;

public static class IQueryableExtension
{
	public static IQueryable<T> TakeLimit<T>(this IQueryable<T> queryable, int limit)
	{
		// has limit
		if (limit > 0)
			return queryable.Take(limit);

		// dosnt has limit
		return queryable;
	}
}
