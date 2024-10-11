using TodoAPI.Data.Events;

namespace TodoAPI.API.Interfaces;

// Save dbContext changes
// can be used by UnitOfWork
public interface ISaveable
{
	event AsyncEventHandler<EventArgs> OnSaveChangesRequested;
}
