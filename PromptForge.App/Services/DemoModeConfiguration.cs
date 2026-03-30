namespace PromptForge.App.Services;

public static class DemoModeConfiguration
{
#if PROMPTFORGE_DEMO
    public const bool IsEnabled = true;
#else
    public const bool IsEnabled = false;
#endif

    public const int MaxDemoCopies = 100;
}
