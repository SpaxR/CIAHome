using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace CIAHome.Client.Services
{
	public class CIAuthenticationStateProvider : AuthenticationStateProvider
	{
		/// <inheritdoc />
		public override Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			return Task.FromResult(new AuthenticationState(new ClaimsPrincipal()));
		}
	}
}