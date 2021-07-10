using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CIA.Infrastructure
{
	public class CIAContext : IdentityDbContext<CIAUser>
	{
		/// <inheritdoc />
		public CIAContext(DbContextOptions options) : base(options) { }
	}
}