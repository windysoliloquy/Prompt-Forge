using System.Security.Cryptography;
using System.Text;
using System.Runtime.CompilerServices;
using PromptForge.Core.Models;

namespace PromptForge.Core.Services;

public static class PromptForgeLicenseCodec
{
    public const string ProductName = "Prompt Forge";

    public static PromptForgeLicenseFile CreateLicenseFile(string purchaserEmail, string licenseId, DateTime issuedUtc)
    {
        var normalizedEmail = purchaserEmail.Trim();
        var normalizedLicenseId = licenseId.Trim();
        var normalizedIssuedUtc = issuedUtc.ToUniversalTime();

        return new PromptForgeLicenseFile
        {
            ProductName = ProductName,
            PurchaserEmail = normalizedEmail,
            LicenseId = normalizedLicenseId,
            IssuedUtc = normalizedIssuedUtc,
            ValidationToken = ComputeValidationToken(ProductName, normalizedEmail, normalizedLicenseId, normalizedIssuedUtc),
        };
    }

    public static bool IsValid(PromptForgeLicenseFile? licenseFile)
    {
        if (licenseFile is null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(licenseFile.ProductName)
            || !string.Equals(licenseFile.ProductName.Trim(), ProductName, StringComparison.Ordinal))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(licenseFile.PurchaserEmail)
            || string.IsNullOrWhiteSpace(licenseFile.LicenseId)
            || licenseFile.IssuedUtc == default
            || string.IsNullOrWhiteSpace(licenseFile.ValidationToken))
        {
            return false;
        }

        var expectedToken = ComputeValidationToken(
            licenseFile.ProductName.Trim(),
            licenseFile.PurchaserEmail.Trim(),
            licenseFile.LicenseId.Trim(),
            licenseFile.IssuedUtc);

        var providedBytes = Encoding.UTF8.GetBytes(licenseFile.ValidationToken.Trim());
        var expectedBytes = Encoding.UTF8.GetBytes(expectedToken);
        return CryptographicOperations.FixedTimeEquals(providedBytes, expectedBytes);
    }

    public static string ComputeValidationToken(string productName, string purchaserEmail, string licenseId, DateTime issuedUtc)
    {
        var payload = $"{productName}|{purchaserEmail}|{licenseId}|{issuedUtc.ToUniversalTime():O}";
        var bytes = Encoding.UTF8.GetBytes(payload);
        var key = ComposeMaterial();

        using var hmac = new HMACSHA256(key);
        return Convert.ToHexString(hmac.ComputeHash(bytes));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static byte[] ComposeMaterial()
    {
        var left = Material.Left;
        var right = Material.Right;
        var key = new byte[left.Length];
        for (var i = 0; i < left.Length; i++)
        {
            key[i] = (byte)(left[i] ^ right[i % right.Length]);
        }

        return key;
    }

    private static class Material
    {
        internal static ReadOnlySpan<byte> Left => [
            0x0A, 0x28, 0x35, 0x37, 0x2A, 0x2E, 0x1C, 0x35,
            0x28, 0x3D, 0x3F, 0x15, 0x3C, 0x3C, 0x36, 0x33,
            0x34, 0x3F, 0x0F, 0x34, 0x36, 0x35, 0x39, 0x31,
            0x05, 0x2C, 0x6B, 0x05, 0x68, 0x6A, 0x68, 0x6C
        ];

        internal static ReadOnlySpan<byte> Right => [0x5A, 0x5A, 0x5A, 0x5A, 0x5A, 0x5A, 0x5A, 0x5A];
    }
}
