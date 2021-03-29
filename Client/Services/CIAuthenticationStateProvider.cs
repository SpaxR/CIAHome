using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using CIAHome.Shared;
using CIAHome.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace CIAHome.Client.Services
{
	public class CIAuthenticationStateProvider : AuthenticationStateProvider
	{
		private const string AuthenticationType = "Custom Authentication";

		private readonly HttpClient _http;

		public CIAuthenticationStateProvider(HttpClient http)
		{
			_http = http;
		}


		/// <inheritdoc />
		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var user = await FetchProfileAsync();

			IIdentity identity = user?.Username is null
				? new GenericIdentity(string.Empty)
				: new ClaimsIdentity(user.Claims.Select(pair => new Claim(pair.Key, pair.Value)), AuthenticationType);

			return new AuthenticationState(new ClaimsPrincipal(identity));
		}


		private async Task<UserProfile> FetchProfileAsync()
		{
			var response = await _http.GetAsync(CIAPath.UserProfile);

			if (response.IsSuccessStatusCode)
			{
				return await response.Content.ReadFromJsonAsync<UserProfile>();
			}

			return null;
		}
	}
}