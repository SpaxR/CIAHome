using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CIAHome.Core
{
	public interface ILoggingEvent : INotification
	{
		string LogMessage { get; }
	}

	public class LoggingEventHandler : INotificationHandler<ILoggingEvent>
	{
		private readonly ILogger _logger;

		public LoggingEventHandler(ILogger logger)
		{
			_logger = logger;
		}


		/// <inheritdoc />
		public Task Handle(ILoggingEvent notification, CancellationToken cancellationToken)
		{
			_logger.LogInformation(notification.LogMessage);
			return Task.CompletedTask;
		}
	}
	
	
}