using System;
using Bunit;
using CIAHome.Client.Components;
using CIAHome.Client.Services;
using CIAHome.Shared.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using Xunit;

namespace CIAHome.Client.Tests
{
	public class LoginFormSpec : TestContext
	{
		private          IRenderedComponent<LoginForm> _sut;
		private readonly Mock<IAuthenticationService>  _authMock = new();

		private IRenderedComponent<LoginForm> SUT
		{
			get
			{
				_sut ??= RenderComponent<LoginForm>();
				return _sut;
			}
		}

		public LoginFormSpec()
		{
			Services.AddScoped(_ => _authMock.Object);
		}

		private void InputData(LoginModel model)
		{
			SUT.FindComponent<MudTextField<string>>(c => c.Instance.Label.Equals("Username"))
			   .Find("input")
			   .Input(model.Username);

			SUT.FindComponent<MudTextField<string>>(c => c.Instance.Label.Equals("Password"))
			   .Find("input")
			   .Input(model.Password);

			SUT.FindComponent<MudCheckBox<bool>>()
			   .Find("input")
			   .Change(model.Remember);

			SUT.WaitForState(() => SUT.FindComponent<MudForm>().Instance.IsValid);
		}


		[Fact]
		public void contains_form()
		{
			var form = SUT.Find("form");

			Assert.NotNull(form);
		}

		[Fact]
		public void contains_input_with_Username_Label()
		{
			var inputs = SUT.FindComponents<MudTextField<string>>();

			Assert.Contains(inputs, field => field.Instance.Label.ToLower().Contains("name"));
		}

		[Fact]
		public void contains_input_with_Password_Label()
		{
			var inputs = SUT.FindComponents<MudTextField<string>>();

			Assert.Contains(inputs, field => field.Instance.Label.ToLower().Contains("pass"));
		}

		[Fact]
		public void password_input_has_type_password()
		{
			var input =
				SUT.FindComponent<MudTextField<string>>(field => field.Instance.Label.Equals("Password"));

			Assert.Equal(InputType.Password, input.Instance.InputType);
		}

		[Fact]
		public void contains_Checkbox_with_RememberMe_Label()
		{
			var checkbox = SUT.FindComponent<MudCheckBox<bool>>();

			Assert.Contains("remember", checkbox.Instance.Label.ToLower());
		}

		[Fact]
		public void contains_Login_button()
		{
			var button = SUT.FindComponent<MudButton>();

			Assert.NotNull(button);
			Assert.Contains("login", button.Markup.ToLower());
		}

		[Fact]
		public void Username_is_required()
		{
			var input = SUT.FindComponent<MudTextField<string>>(
				field => field.Instance.Label.Equals("Username"));

			Assert.True(input.Instance.Required);
		}

		[Fact]
		public void Password_is_required()
		{
			var input = SUT.FindComponent<MudTextField<string>>(
				field => field.Instance.Label.Equals("Password"));

			Assert.True(input.Instance.Required);
		}

		[Fact]
		public void Click_button_calls_Login()
		{
			InputData(new LoginModel
			{
				Username = "USERNAME",
				Password = "PASSWORD"
			});

			SUT.FindComponent<MudButton>().Find("button").Click();

			_authMock.Verify(auth => auth.Login(It.IsAny<LoginModel>()));
		}

		[Fact]
		public void Button_is_Disabled_when_form_is_invalid()
		{
			var form   = SUT.FindComponent<MudForm>();
			var button = SUT.FindComponent<MudButton>();

			form.Instance.Validate();

			Assert.True(button.Instance.Disabled);
		}

		[Fact]
		public void Successful_Login_triggers_OnSuccess()
		{
			bool hasBeenCalled = false;
			SUT.Instance.OnSuccess = EventCallback.Factory.Create(this, () => hasBeenCalled = true);
			InputData(new LoginModel
			{
				Username = "USERNAME",
				Password = "PASSWORD"
			});

			SUT.FindComponent<MudButton>().Find("button").Click();

			Assert.True(hasBeenCalled);
		}

		[Fact]
		public void Failing_Login_triggers_OnFailure()
		{
			bool hasBeenCalled = false;
			_authMock.Setup(auth => auth.Login(It.IsAny<LoginModel>())).Throws<Exception>();
			SUT.Instance.OnFailure = EventCallback.Factory.Create(this, () => hasBeenCalled = true);
			InputData(new LoginModel
			{
				Username = "USERNAME",
				Password = "PASSWORD"
			});

			SUT.FindComponent<MudButton>().Find("button").Click();

			Assert.True(hasBeenCalled);
		}

		[Fact]
		public void Login_provides_UserInput_in_model()
		{
			LoginModel result = null;
			_authMock.Setup(auth => auth.Login(It.IsAny<LoginModel>()))
					 .Callback<LoginModel>(para => result = para);

			var model = new LoginModel
			{
				Username = "USERNAME",
				Password = "PASSWORD",
				Remember = true
			};
			InputData(model);


			SUT.FindComponent<MudButton>().Find("button").Click();

			Assert.Equal(model.Username, result.Username);
			Assert.Equal(model.Password, result.Password);
			Assert.Equal(model.Remember, result.Remember);
		}
	}
}