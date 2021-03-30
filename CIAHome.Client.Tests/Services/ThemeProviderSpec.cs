using System;
using CIAHome.Client.Services;
using MudBlazor;
using Xunit;

namespace CIAHome.Client.Tests.Services
{
	public class ThemeProviderSpec
	{
		private readonly ThemeProvider _sut = new();


		[Fact]
		public void CurrentTheme_defaults_to_dark()
		{
			Assert.Equal(ThemeProvider.DarkTheme, _sut.CurrentTheme);
		}


		[Fact]
		public void ChangeTheme_changes_CurrentTheme()
		{
			var theme = new MudTheme();

			_sut.ChangeTheme(theme);

			Assert.Equal(theme, _sut.CurrentTheme);
		}

		[Fact]
		public void ChangeTheme_triggers_OnChange()
		{
			Assert.Raises<EventArgs>(handler => _sut.ThemeChanged += handler,
						  handler => _sut.ThemeChanged -= handler,
						  () => _sut.ChangeTheme(_sut.CurrentTheme));
		}
	}
}