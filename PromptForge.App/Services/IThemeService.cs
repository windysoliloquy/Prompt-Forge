namespace PromptForge.App.Services;

public interface IThemeService
{
    IReadOnlyList<string> AvailableThemeNames { get; }
    string CurrentThemeName { get; }
    void ApplyTheme(string themeName);
}
