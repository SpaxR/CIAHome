using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Blazored.LocalStorage;
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
		private const string AuthenticationType = "Custom Authentication";

		private readonly AuthenticationState  _anonymous = new(new ClaimsPrincipal(new GenericIdentity(string.Empty)));
		private readonly IHttpClientFactory   _httpClientFactory;
		private readonly ILocalStorageService _storage;

		public CIAuthenticationService(IHttpClientFactory httpClientFactory, ILocalStorageService storage)
		{
			_httpClientFactory = httpClientFactory;
			_storage           = storage;
		}

		/// <inheritdoc />
		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var user = await _storage.GetItemAsync<UserProfile>(nameof(UserProfile));

			if (user == null)
			{
				return _anonymous;
			}

			var claims = user.Claims.Select(pair => new Claim(pair.Key, pair.Value));

			return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims, AuthenticationType)));
		}

		public async Task Login(LoginModel model)
		{
			var client   = _httpClientFactory.CreateClient();
			var response = await client.PostAsJsonAsync(CIAPath.Login, model);
			response.EnsureSuccessStatusCode();
			await User();
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}

		public async Task Logout()
		{
			var client   = _httpClientFactory.CreateClient();
			var response = await client.GetAsync(CIAPath.Logout);
			response.EnsureSuccessStatusCode();
			await _storage.RemoveItemAsync(nameof(UserProfile));
			NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}

		public async Task<UserProfile> User()
		{
			var client   = _httpClientFactory.CreateClient();
			var response = await client.GetAsync(CIAPath.UserProfile);
			response.EnsureSuccessStatusCode();

			var profile = await response.Content.ReadFromJsonAsync<UserProfile>();
			await _storage.SetItemAsync(nameof(UserProfile), profile);
			return profile;
		}
	}
}