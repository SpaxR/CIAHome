using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CIAHome.Server.Data
{
	public class CIADbContext : IdentityDbContext<CIAUser>
	{
		/// <inheritdoc />
		public CIADbContext(DbContextOptions options) : base(options) { }
	}
}