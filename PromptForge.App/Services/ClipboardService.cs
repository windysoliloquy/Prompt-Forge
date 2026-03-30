using System.Windows;

namespace PromptForge.App.Services;

public sealed class ClipboardService : IClipboardService
{
    public bool TrySetText(string text)
    {
        try
        {
            Clipboard.SetText(text ?? string.Empty);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
