using System.Windows;

namespace PromptForge.App.Services;

public sealed class LicenseDialogService : ILicenseDialogService
{
    private readonly ILicenseService _licenseService;

    public LicenseDialogService(ILicenseService licenseService)
    {
        _licenseService = licenseService;
    }

    public bool ShowDialog()
    {
        var dialog = new UnlockWindow(_licenseService)
        {
            Owner = Application.Current?.MainWindow
        };

        return dialog.ShowDialog() == true;
    }
}
