using System.Diagnostics;
using System.Windows;
using Microsoft.Win32;
using PromptForge.App.Services;

namespace PromptForge.App;

public partial class UnlockWindow : Window
{
    private const string PurchaseEmail = "windysoliloquy@gmail.com";
    private readonly ILicenseService _licenseService;

    public UnlockWindow(ILicenseService licenseService)
    {
        _licenseService = licenseService;
        InitializeComponent();
        RefreshState();
    }

    private void EmailToPurchase_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var uri =
                "mailto:windysoliloquy@gmail.com?subject=Prompt%20Forge%20Full%20Purchase&body=Name:%0D%0AEmail:%0D%0A%0D%0AI%20would%20like%20to%20purchase%20Prompt%20Forge%20Full.";
            Process.Start(new ProcessStartInfo(uri) { UseShellExecute = true });
        }
        catch
        {
            MessageBox.Show(
                this,
                $"Your default mail client could not be opened automatically. Please email {PurchaseEmail}.",
                "Email Purchase",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }

    private void ImportUnlockFile_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Title = "Import Prompt Forge Unlock File",
            Filter = "Prompt Forge Unlock File (*.json)|*.json|All Files (*.*)|*.*",
            CheckFileExists = true,
            Multiselect = false,
        };

        if (dialog.ShowDialog(this) != true)
        {
            return;
        }

        var result = _licenseService.ImportUnlockFile(dialog.FileName);
        StatusMessageTextBlock.Text = result.Message;
        RefreshState();

        MessageBox.Show(
            this,
            result.Message,
            result.Success ? "Activation Result" : "Unlock Failed",
            MessageBoxButton.OK,
            result.Success ? MessageBoxImage.Information : MessageBoxImage.Warning);

        if (result.Success)
        {
            DialogResult = true;
            Close();
        }
    }

    private void Close_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void RefreshState()
    {
        if (_licenseService.IsUnlocked)
        {
            StatusHeadlineTextBlock.Text = "Version: Full";
            StatusBodyTextBlock.Text = "Prompt Forge Full is active on this machine.";
            LicenseSummaryTextBlock.Text = BuildLicenseSummary();
            return;
        }

        StatusHeadlineTextBlock.Text = _licenseService.IsDemoBuild ? "Version: Demo" : "Version: Full";
        StatusBodyTextBlock.Text = _licenseService.IsDemoBuild
            ? "Demo mode is active until a valid unlock file is imported."
            : "This build is already running without demo restrictions.";
        LicenseSummaryTextBlock.Text = "No local unlock has been imported yet.";
    }

    private string BuildLicenseSummary()
    {
        var email = string.IsNullOrWhiteSpace(_licenseService.PurchaserEmail)
            ? "Activated locally."
            : $"Licensed to {_licenseService.PurchaserEmail}.";
        var licenseId = string.IsNullOrWhiteSpace(_licenseService.LicenseId)
            ? string.Empty
            : $" License ID: {_licenseService.LicenseId}.";
        var issuedUtc = _licenseService.IssuedUtc is null
            ? string.Empty
            : $" Issued: {_licenseService.IssuedUtc.Value:yyyy-MM-dd HH:mm} UTC.";

        return $"{email}{licenseId}{issuedUtc}".Trim();
    }
}
