using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using CIAHome.Client.Services;
using CIAHome.Shared;
using CIAHome.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Moq;
using Moq.Contrib.HttpClient;
using Newtonsoft.Json;
using Xunit;

namespace CIAHome.Client.Tests
{
	public class CIAuthenticationServiceSpec
	{
		private readonly Uri _baseAddress = new("http://localhost");

		private readonly Mock<HttpMessageHandler>   _httpHandler = new();
		private readonly Mock<ILocalStorageService> _storageMock = new();

		private readonly CIAuthenticationService _sut;

		public CIAuthenticationServiceSpec()
		{
			_sut = new CIAuthenticationService(_httpHandler.CreateClientFactory(_baseAddress), _storageMock.Object);
		}

		private void SetupLogin(bool setupProfile = false)
		{
			_httpHandler.SetupRequest(HttpMethod.Post, new Uri(_baseAddress, CIAPath.Login))
						.ReturnsResponse(HttpStatusCode.OK);

			if (setupProfile)
			{
				SetupUserProfile(new UserProfile());
				SetupUserProfileStorage(new UserProfile());
			}
		}

		private void SetupUserProfile(UserProfile profile)
		{
			_httpHandler.SetupRequest(HttpMethod.Get, new Uri(_baseAddress, CIAPath.UserProfile))
						.ReturnsResponse(JsonConvert.SerializeObject(profile));
		}

		private void SetupUserProfileStorage(UserProfile profile)
		{
			_storageMock.Setup(storage => storage.GetItemAsync<UserProfile>(nameof(UserProfile)))
						.ReturnsAsync(profile);
		}

		private void SetupLogout()
		{
			_httpHandler.SetupRequest(HttpMethod.Get, new Uri(_baseAddress, CIAPath.Logout))
						.ReturnsResponse(HttpStatusCode.OK);
		}

		[Fact]
		public async Task Logout_calls_API()
		{
			SetupLogout();

			await _sut.Logout();

			_httpHandler.VerifyRequest(HttpMethod.Get, new Uri(_baseAddress, CIAPath.Logout));
		}

		[Fact]
		public async Task Logout_without_Success_throws_HttpRequestException()
		{
			_httpHandler.SetupAnyRequest().ReturnsResponse(HttpStatusCode.Forbidden);

			await Assert.ThrowsAsync<HttpRequestException>(() => _sut.Logout());
		}

		[Fact]
		public async Task Logout_without_Success_Removes_UserProfile_from_Storage()
		{
			_httpHandler.SetupAnyRequest().ReturnsResponse(HttpStatusCode.Forbidden);

			await Assert.ThrowsAsync<HttpRequestException>(() => _sut.Logout());

			_storageMock.Verify(storage => storage.RemoveItemAsync(nameof(UserProfile)));
		}

		[Fact]
		public async Task Logout_withUnavailableServer_Removes_UserProfile_from_Storage()
		{
			await Assert.ThrowsAsync<InvalidOperationException>(() => _sut.Logout());

			_storageMock.Verify(storage => storage.RemoveItemAsync(nameof(UserProfile)));
		}

		[Fact]
		public async Task Logout_triggers_AuthenticationStateChanged()
		{
			Task<AuthenticationState> result = null;
			SetupLogout();
			_sut.AuthenticationStateChanged += state => result = state;

			await _sut.Logout();

			Assert.NotNull(result);
		}

		[Fact]
		public async Task Logout_removes_UserProfile_from_Storage()
		{
			SetupLogout();

			await _sut.Logout();

			_storageMock.Verify(storage => storage.RemoveItemAsync(nameof(UserProfile)));
		}

		[Fact]
		public async Task Login_sends_LoginModel_to_API()
		{
			SetupLogin(true);
			var model = new LoginModel();

			await _sut.Login(model);

			_httpHandler.VerifyRequest(HttpMethod.Post,
									   new Uri(_baseAddress, CIAPath.Login),
									   async message =>
									   {
										   string content = await message.Content!.ReadAsStringAsync();
										   Assert.Equal(JsonConvert.SerializeObject(model).ToLower(),
														content.ToLower());
										   // Assert.Equal(model, await message.Content.ReadFromJsonAsync<LoginModel>());
										   return true;
									   });
		}

		[Fact]
		public async Task Login_triggers_AuthenticationStateChanged()
		{
			SetupLogin(true);
			Task<AuthenticationState> result = null;
			_sut.AuthenticationStateChanged += state => result = state;

			await _sut.Login(new LoginModel());

			Assert.NotNull(result);
		}

		[Fact]
		public async Task Login_without_Success_Success_throws_HttpRequestException()
		{
			_httpHandler.SetupAnyRequest().ReturnsResponse(HttpStatusCode.Forbidden);

			await Assert.ThrowsAsync<HttpRequestException>(() => _sut.Login(new LoginModel()));
		}

		[Fact]
		public async Task User_calls_API()
		{
			SetupUserProfile(new UserProfile());

			await _sut.User();

			_httpHandler.VerifyRequest(HttpMethod.Get, new Uri(_baseAddress, CIAPath.UserProfile));
		}

		[Fact]
		public async Task User_returns_UserProfile_from_Response()
		{
			var profile = new UserProfile();
			SetupUserProfile(profile);

			var result = await _sut.User();

			Assert.Equal(JsonConvert.SerializeObject(profile), JsonConvert.SerializeObject(result));
		}

		[Fact]
		public async Task User_without_Success_throws_HttpRequestException()
		{
			_httpHandler.SetupAnyRequest().ReturnsResponse(HttpStatusCode.Forbidden);

			await Assert.ThrowsAsync<HttpRequestException>(() => _sut.User());
		}

		[Fact]
		public async Task User_saves_UserData_in_LocalStorage()
		{
			SetupUserProfile(new UserProfile());

			await _sut.User();

			_storageMock.Verify(storage => storage.SetItemAsync(nameof(UserProfile), It.IsAny<UserProfile>()));
		}

		[Fact]
		public async Task AuthenticationState_returns_Unauthenticated_Principal()
		{
			var state = await _sut.GetAuthenticationStateAsync();
			var user  = state.User;

			Assert.False(user.Identity?.IsAuthenticated);
		}

		[Fact]
		public async Task AuthenticationState_after_Login_returns_Authenticated_Principal()
		{
			SetupLogin(true);

			await _sut.Login(new LoginModel());
			var result = await _sut.GetAuthenticationStateAsync();

			Assert.True(result.User.Identity?.IsAuthenticated);
		}

		[Fact]
		public async Task AuthenticationState_after_Logout_returns_Unauthenticated_Principal()
		{
			SetupLogin();
			SetupUserProfile(new UserProfile());
			SetupLogout();

			await _sut.Login(new LoginModel());
			await _sut.Logout();
			var result = await _sut.GetAuthenticationStateAsync();

			Assert.False(result.User.Identity?.IsAuthenticated);
		}

		[Fact]
		public async Task AuthenticationState_withUserInStorage_returns_Authenticated_Principal()
		{
			SetupUserProfileStorage(new UserProfile());

			var result = await _sut.GetAuthenticationStateAsync();

			Assert.True(result.User.Identity?.IsAuthenticated);
		}

		[Fact]
		public async Task AuthenticationState_User_contains_Claims_from_Profile()
		{
			var profile = new UserProfile
			{
				Username = "SOME NAME",
				Claims = new Dictionary<string, string>
				{
					{"Claim 1", "Value 1"},
					{"Claim 2", "Value 2"},
				}
			};
			SetupUserProfileStorage(profile);

			var result = await _sut.GetAuthenticationStateAsync();

			var claims = result.User.Claims.ToDictionary(c => c.Type, c => c.Value);

			Assert.Equal(profile.Claims, claims);
		}
	}
}