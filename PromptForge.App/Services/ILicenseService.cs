namespace PromptForge.App.Services;

public interface ILicenseService
{
    bool IsDemoBuild { get; }
    bool IsUnlocked { get; }
    string? PurchaserEmail { get; }
    string? LicenseId { get; }
    DateTime? IssuedUtc { get; }
    UnlockImportResult ImportUnlockFile(string filePath);
}
