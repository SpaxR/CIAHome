using Microsoft.EntityFrameworkCore;

namespace CIAHome.Core
{
	public interface IUserContext
	{
		DbSet<User> Users { get; set; }
	}
}