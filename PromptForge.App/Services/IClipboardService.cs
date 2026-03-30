namespace PromptForge.App.Services;

public interface IClipboardService
{
    bool TrySetText(string text);
}
