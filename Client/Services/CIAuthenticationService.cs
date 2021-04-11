using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using CIAHome.Shared;
using CIAHome.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace CIAHome.Client.Services
{
	public interface IAuthenticationService
	{
		Task Login(LoginModel model);
		Task Logout();

		Task<UserProfile> User();
	}

	public class CIAuthenticationService : AuthenticationStateProvider, IAuthenticationService
	{
		private readonly IHttpClientFactory _httpClientFactory;
		private const    string             AuthenticationType = "Custom Authentication";
		private          bool               _isLoggedIn        = false;

		public CIAuthenticationService(IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory;
		}

		/// <inheritdoc />
		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			await Task.CompletedTask;
			return new(new ClaimsPrincipal(new ClaimsIdentity(_isLoggedIn ? AuthenticationType : string.Empty)));

			// var user = await FetchProfileAsync();
			// IIdentity identity = user?.Username is null
			// 						 ? new GenericIdentity(string.Empty)
			// 						 : new ClaimsIdentity(user.Claims.Select(pair => new Claim(pair.Key, pair.Value)),
			// 											  AuthenticationType);
			//
			// return new AuthenticationState(new ClaimsPrincipal(identity));
		}

		public async Task Login(LoginModel model)
		{
			var client   = _httpClientFactory.CreateClient();
			var response = await client.PostAsJsonAsync(CIAPath.Login, model);
			response.EnsureSuccessStatusCode();
			_isLoggedIn = true;
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}

		public async Task Logout()
		{
			var client   = _httpClientFactory.CreateClient();
			var response = await client.GetAsync(CIAPath.Logout);
			response.EnsureSuccessStatusCode();
			_isLoggedIn = false;
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}

		public async Task<UserProfile> User()
		{
			var client   = _httpClientFactory.CreateClient();
			var response = await client.GetAsync(CIAPath.UserProfile);
			response.EnsureSuccessStatusCode();

			return await response.Content.ReadFromJsonAsync<UserProfile>();
		}
	}
}