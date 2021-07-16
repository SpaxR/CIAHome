using System;
using MudBlazor;

namespace WebUI.Client.Services
{
	public interface IThemeProvider
	{
		event EventHandler<EventArgs> ThemeChanged;

		MudTheme CurrentTheme { get; }
		void     ChangeTheme(MudTheme theme);

	}
	
	public class ThemeProvider : IThemeProvider
	{
		public MudTheme CurrentTheme { get; private set; } = DarkTheme;

		public event EventHandler<EventArgs> ThemeChanged;
		
		public static readonly MudTheme LightTheme = new()
		{
			Palette = new Palette
			{
				// Primary          = "#",
				// Secondary        = "#",
				// Tertiary         = "#",
				// AppbarBackground = "#",

				Background = "#FAFAFA",
				Surface    = "#FFF",
			}
		};

		// Darcula
		public static readonly MudTheme DarkTheme = new()
		{
			Palette = new Palette
			{
				Primary   = "#ac0909",
				Secondary = "#36a546",
				Tertiary  = "#f1eb7f",

				AppbarBackground = "#303030", // BG-2
				Background       = "#2B2B2B", // BG-1
				Surface          = "#303030", // BG-2
				DrawerBackground = "#303030", // BG-2

				TextPrimary   = "#A9B7C6", // FG-1
				TextSecondary = "#CCCCCC", // FG-2

				Dark         = "FF0000",
				TextDisabled = "FF0000",

				ActionDisabled = "#8e9292", // Comment
				Divider        = "#3C3F41", // Line-BG
			},
		};

		public void ChangeTheme(MudTheme theme)
		{
			CurrentTheme = theme;
			ThemeChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}