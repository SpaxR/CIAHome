namespace CIA.Infrastructure
{
	// public class TodoListEventHandler : INotificationHandler<TodoListCreated>,
	// 									INotificationHandler<TodoListEdited>,
	// 									INotificationHandler<TodoListDeleted>
	// {
	// 	private readonly TodoContext _context;
	//
	// 	public TodoListEventHandler(TodoContext context)
	// 	{
	// 		_context = context;
	// 	}
	//
	// 	/// <inheritdoc />
	// 	public Task Handle(TodoListCreated notification, CancellationToken cancellationToken)
	// 	{
	// 		_context.TodoLists.Add(notification.List);
	// 		_context.SaveChanges();
	// 		return Task.CompletedTask;
	// 	}
	//
	// 	/// <inheritdoc />
	// 	public Task Handle(TodoListEdited notification, CancellationToken cancellationToken)
	// 	{
	// 		_context.TodoLists.Update(notification.List);
	// 		_context.SaveChanges();
	// 		return Task.CompletedTask;
	// 	}
	//
	// 	/// <inheritdoc />
	// 	public Task Handle(TodoListDeleted notification, CancellationToken cancellationToken)
	// 	{
	// 		_context.Remove(notification.List);
	// 		_context.SaveChanges();
	// 		return Task.CompletedTask;
	// 	}
	// }
}