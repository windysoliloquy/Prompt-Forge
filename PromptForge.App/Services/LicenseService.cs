using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using PromptForge.Core.Models;
using PromptForge.Core.Services;

namespace PromptForge.App.Services;

public sealed class LicenseService : ILicenseService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
    };

    private readonly string _statePath;
    private LicenseState _state;

    public LicenseService()
    {
        IsDemoBuild = DemoModeConfiguration.IsEnabled;

        var appDataDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "PromptForge");
        Directory.CreateDirectory(appDataDirectory);

        _statePath = Path.Combine(appDataDirectory, "license-state.json");
        _state = LoadState();
    }

    public bool IsDemoBuild { get; }

    public bool IsUnlocked => _state.IsUnlocked;

    public string? PurchaserEmail => _state.PurchaserEmail;

    public string? LicenseId => _state.LicenseId;

    public DateTime? IssuedUtc => _state.IssuedUtc;

    public UnlockImportResult ImportUnlockFile(string filePath)
    {
        PromptForgeLicenseFile? unlockFile;

        try
        {
            var json = File.ReadAllText(filePath);
            unlockFile = JsonSerializer.Deserialize<PromptForgeLicenseFile>(json, JsonOptions);
        }
        catch
        {
            return new UnlockImportResult(false, "The selected unlock file could not be read.");
        }

        if (unlockFile is null)
        {
            return new UnlockImportResult(false, "The selected unlock file is empty or invalid.");
        }

        if (!PromptForgeLicenseCodec.IsValid(unlockFile))
        {
            return new UnlockImportResult(false, "Unlock file validation failed. Please check the file and try again.");
        }

        try
        {
            _state = new LicenseState
            {
                IsUnlocked = true,
                PurchaserEmail = unlockFile.PurchaserEmail.Trim(),
                LicenseId = unlockFile.LicenseId.Trim(),
                IssuedUtc = unlockFile.IssuedUtc,
                ActivatedUtc = DateTime.UtcNow,
            };

            File.WriteAllText(_statePath, JsonSerializer.Serialize(_state, JsonOptions));
        }
        catch
        {
            return new UnlockImportResult(false, "Activation could not be saved locally. Prompt Forge remains in its current state.");
        }

        var cleanupSucceeded = TryDestroyImportedFile(filePath);
        var message = cleanupSucceeded
            ? "Activation succeeded. Prompt Forge Full is now unlocked on this machine."
            : "Activation succeeded. Prompt Forge Full is now unlocked, but the original unlock file could not be removed automatically.";

        return new UnlockImportResult(true, message, cleanupSucceeded);
    }

    private LicenseState LoadState()
    {
        try
        {
            if (File.Exists(_statePath))
            {
                var json = File.ReadAllText(_statePath);
                var state = JsonSerializer.Deserialize<LicenseState>(json, JsonOptions);
                if (state is not null)
                {
                    return state;
                }
            }
        }
        catch
        {
        }

        return new LicenseState();
    }

    private static bool TryDestroyImportedFile(string filePath)
    {
        try
        {
            var fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                return true;
            }

            if (fileInfo.IsReadOnly)
            {
                fileInfo.IsReadOnly = false;
            }

            var length = fileInfo.Length;
            if (length > 0)
            {
                using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Write, FileShare.None);
                var buffer = new byte[Math.Min(4096, (int)Math.Max(1, length))];
                long remaining = length;

                while (remaining > 0)
                {
                    RandomNumberGenerator.Fill(buffer);
                    var count = (int)Math.Min(buffer.Length, remaining);
                    stream.Write(buffer, 0, count);
                    remaining -= count;
                }

                stream.Flush(true);
                stream.SetLength(0);
            }

            File.Delete(filePath);
            return !File.Exists(filePath);
        }
        catch
        {
            return false;
        }
    }

    private sealed class LicenseState
    {
        public bool IsUnlocked { get; set; }

        public string? PurchaserEmail { get; set; }

        public string? LicenseId { get; set; }

        public DateTime? IssuedUtc { get; set; }

        public DateTime? ActivatedUtc { get; set; }
    }
}
