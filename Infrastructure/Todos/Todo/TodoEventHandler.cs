namespace CIA.Infrastructure
{
	// public class TodoEventHandler : INotificationHandler<TodoItemCreated>,
	// 								INotificationHandler<TodoItemEdited>,
	// 								INotificationHandler<TodoItemDeleted>
	// {
	// 	private readonly TodoContext _context;
	//
	// 	public TodoEventHandler(TodoContext context)
	// 	{
	// 		_context = context;
	// 	}
	//
	// 	/// <inheritdoc />
	// 	public Task Handle(TodoItemCreated notification, CancellationToken cancellationToken)
	// 	{
	// 		_context.Todos.Add(notification.Item);
	// 		_context.SaveChanges();
	// 		return Task.CompletedTask;
	// 	}
	//
	// 	/// <inheritdoc />
	// 	public Task Handle(TodoItemEdited notification, CancellationToken cancellationToken)
	// 	{
	// 		_context.Todos.Update(notification.Item);
	// 		_context.SaveChanges();
	// 		return Task.CompletedTask;
	// 	}
	//
	// 	/// <inheritdoc />
	// 	public Task Handle(TodoItemDeleted notification, CancellationToken cancellationToken)
	// 	{
	// 		_context.Remove(notification.Item);
	// 		_context.SaveChanges();
	// 		return Task.CompletedTask;
	// 	}
	// }
}