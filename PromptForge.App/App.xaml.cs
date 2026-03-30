using System.Windows;
using PromptForge.App.Services;
using PromptForge.App.ViewModels;

namespace PromptForge.App;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var artistProfileService = new ArtistProfileService();
        var promptBuilder = new PromptBuilderService(artistProfileService);
        var presetStorage = new PresetStorageService();
        var clipboardService = new ClipboardService();
        var licenseService = new LicenseService();
        var demoModeService = new DemoModeService(licenseService);
        var licenseDialogService = new LicenseDialogService(licenseService);
        var themeService = new ThemeService();
        themeService.ApplyTheme(themeService.CurrentThemeName);

        var mainWindow = new MainWindow
        {
            DataContext = new MainWindowViewModel(promptBuilder, presetStorage, clipboardService, demoModeService, licenseService, licenseDialogService, artistProfileService, themeService)
        };

        MainWindow = mainWindow;
        mainWindow.Show();
    }
}


