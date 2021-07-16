using MediatR;
using Microsoft.Extensions.Logging;

namespace CIAHome.Core
{
	public interface INotificationEvent : INotification
	{
		string   Message  { get; }
		LogLevel Severity { get; }
	}
}