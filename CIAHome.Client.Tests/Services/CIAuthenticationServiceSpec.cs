using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CIAHome.Client.Services;
using CIAHome.Shared;
using CIAHome.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Moq;
using Moq.Contrib.HttpClient;
using Newtonsoft.Json;
using Xunit;

namespace CIAHome.Client.Tests.Services
{
	public class CIAuthenticationServiceSpec
	{
		private readonly Uri _baseAddress = new("http://localhost");

		private readonly Mock<HttpMessageHandler> _httpHandler = new();

		private readonly CIAuthenticationService _sut;

		public CIAuthenticationServiceSpec()
		{
			_sut = new CIAuthenticationService(_httpHandler.CreateClientFactory(_baseAddress));
		}

		[Fact]
		public async Task Logout_calls_API()
		{
			_httpHandler.SetupAnyRequest().ReturnsResponse(HttpStatusCode.OK);

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
		public async Task Logout_triggers_AuthenticationStateChanged()
		{
			Task<AuthenticationState> result = null;
			_httpHandler.SetupAnyRequest().ReturnsResponse(HttpStatusCode.OK);
			_sut.AuthenticationStateChanged += state => result = state;
			
			await _sut.Logout();

			Assert.NotNull(result);
		}
		
		[Fact]
		public async Task Login_sends_LoginModel_to_API()
		{
			var model = new LoginModel();
			_httpHandler.SetupAnyRequest().ReturnsResponse(HttpStatusCode.OK);

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
			Task<AuthenticationState> result = null;
			_httpHandler.SetupAnyRequest().ReturnsResponse(HttpStatusCode.OK);
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
			_httpHandler.SetupAnyRequest().ReturnsResponse(JsonConvert.SerializeObject(new UserProfile()));

			await _sut.User();

			_httpHandler.VerifyRequest(HttpMethod.Get, new Uri(_baseAddress, CIAPath.UserProfile));
		}

		[Fact]
		public async Task User_returns_UserProfile_from_Response()
		{
			var profile = new UserProfile();

			_httpHandler.SetupRequest(HttpMethod.Get, new Uri(_baseAddress, CIAPath.UserProfile))
						.ReturnsResponse(JsonConvert.SerializeObject(profile));

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
		public async Task AuthenticationState_returns_Unauthenticated_Principal()
		{
			var state = await _sut.GetAuthenticationStateAsync();
			var user  = state.User;

			Assert.False(user.Identity?.IsAuthenticated);
		}

		[Fact]
		public async Task AuthenticationState_after_Login_returns_Authenticated_Principal()
		{
			_httpHandler.SetupAnyRequest().ReturnsResponse(HttpStatusCode.OK);

			await _sut.Login(new LoginModel());
			var result = await _sut.GetAuthenticationStateAsync();

			Assert.True(result.User.Identity?.IsAuthenticated);
		}

		[Fact]
		public async Task AuthenticationState_after_Logout_returns_Unauthenticated_Principal()
		{
			_httpHandler.SetupAnyRequest().ReturnsResponse(HttpStatusCode.OK);

			await _sut.Login(new LoginModel());
			await _sut.Logout();
			var result = await _sut.GetAuthenticationStateAsync();

			Assert.False(result.User.Identity?.IsAuthenticated);
		}
	}
}