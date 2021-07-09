using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CIAHome.Server.Data
{
	public class CIAContext : IdentityDbContext<CIAUser>
	{
		/// <inheritdoc />
		public CIAContext(DbContextOptions options) : base(options) { }
	}
}