using System.IO;
using System.Text.Json;

namespace PromptForge.App.Services;

public sealed class DemoModeService : IDemoModeService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
    };

    private readonly ILicenseService _licenseService;
    private readonly string _statePath;
    private int _remainingDemoCopies;

    public DemoModeService(ILicenseService licenseService)
    {
        _licenseService = licenseService;
        IsDemoBuild = DemoModeConfiguration.IsEnabled;
        MaxDemoCopies = DemoModeConfiguration.MaxDemoCopies;

        var appDataDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "PromptForge");
        Directory.CreateDirectory(appDataDirectory);

        _statePath = Path.Combine(appDataDirectory, "demo-state.json");
        _remainingDemoCopies = IsDemoBuild ? LoadRemainingDemoCopies() : MaxDemoCopies;
    }

    public bool IsDemoBuild { get; }

    public bool IsDemoMode => IsDemoBuild && !_licenseService.IsUnlocked;

    public int MaxDemoCopies { get; }

    public int RemainingDemoCopies => IsDemoBuild ? _remainingDemoCopies : MaxDemoCopies;

    public bool HasRemainingCopies => !IsDemoMode || RemainingDemoCopies > 0;

    public bool TryConsumeCopy()
    {
        if (!IsDemoMode)
        {
            return true;
        }

        if (_remainingDemoCopies <= 0)
        {
            return false;
        }

        _remainingDemoCopies--;
        SaveState(_remainingDemoCopies);
        return true;
    }

    private int LoadRemainingDemoCopies()
    {
        try
        {
            if (File.Exists(_statePath))
            {
                var json = File.ReadAllText(_statePath);
                var state = JsonSerializer.Deserialize<DemoModeState>(json, JsonOptions);
                if (state is not null)
                {
                    return Math.Clamp(state.RemainingDemoCopies, 0, MaxDemoCopies);
                }
            }
        }
        catch
        {
        }

        SaveState(MaxDemoCopies);
        return MaxDemoCopies;
    }

    private void SaveState(int remainingDemoCopies)
    {
        try
        {
            var state = new DemoModeState
            {
                RemainingDemoCopies = remainingDemoCopies,
                MaxDemoCopies = MaxDemoCopies,
                UpdatedAtUtc = DateTime.UtcNow,
            };

            File.WriteAllText(_statePath, JsonSerializer.Serialize(state, JsonOptions));
        }
        catch
        {
        }
    }

    private sealed class DemoModeState
    {
        public int RemainingDemoCopies { get; set; }

        public int MaxDemoCopies { get; set; }

        public DateTime UpdatedAtUtc { get; set; }
    }
}
