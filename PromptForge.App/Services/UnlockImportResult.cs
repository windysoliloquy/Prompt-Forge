namespace PromptForge.App.Services;

public sealed record UnlockImportResult(bool Success, string Message, bool CleanupSucceeded = false);
