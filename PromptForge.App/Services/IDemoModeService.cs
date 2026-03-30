namespace PromptForge.App.Services;

public interface IDemoModeService
{
    bool IsDemoBuild { get; }
    bool IsDemoMode { get; }
    int MaxDemoCopies { get; }
    int RemainingDemoCopies { get; }
    bool HasRemainingCopies { get; }
    bool TryConsumeCopy();
}
