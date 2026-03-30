[CmdletBinding()]
param(
    [string]$OutputDirectory
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

Add-Type -AssemblyName System.Drawing

if ([string]::IsNullOrWhiteSpace($OutputDirectory)) {
    $OutputDirectory = Join-Path $PSScriptRoot '..\PromptForge.App\Assets'
}

function New-RoundedRectanglePath {
    param(
        [System.Drawing.RectangleF]$Bounds,
        [float]$Radius
    )

    $diameter = $Radius * 2
    $path = New-Object System.Drawing.Drawing2D.GraphicsPath
    $path.AddArc($Bounds.X, $Bounds.Y, $diameter, $diameter, 180, 90)
    $path.AddArc($Bounds.Right - $diameter, $Bounds.Y, $diameter, $diameter, 270, 90)
    $path.AddArc($Bounds.Right - $diameter, $Bounds.Bottom - $diameter, $diameter, $diameter, 0, 90)
    $path.AddArc($Bounds.X, $Bounds.Bottom - $diameter, $diameter, $diameter, 90, 90)
    $path.CloseFigure()
    return $path
}

function New-IconBitmap {
    param([int]$Size)

    $bitmap = New-Object System.Drawing.Bitmap -ArgumentList $Size, $Size
    $graphics = [System.Drawing.Graphics]::FromImage($bitmap)
    $graphics.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::HighQuality
    $graphics.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
    $graphics.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
    $graphics.TextRenderingHint = [System.Drawing.Text.TextRenderingHint]::AntiAliasGridFit
    $graphics.Clear([System.Drawing.Color]::Transparent)

    $padding = [Math]::Max(2, [int]($Size * 0.09))
    $radius = [Math]::Max(4, $Size * 0.22)
    $panel = New-Object System.Drawing.RectangleF -ArgumentList $padding, $padding, ($Size - ($padding * 2)), ($Size - ($padding * 2))
    $path = New-RoundedRectanglePath -Bounds $panel -Radius $radius

    $backgroundBrush = New-Object System.Drawing.Drawing2D.LinearGradientBrush -ArgumentList `
        (New-Object System.Drawing.Point -ArgumentList 0, 0),
        (New-Object System.Drawing.Point -ArgumentList $Size, $Size),
        ([System.Drawing.Color]::FromArgb(255, 31, 38, 49)),
        ([System.Drawing.Color]::FromArgb(255, 69, 83, 99))
    $graphics.FillPath($backgroundBrush, $path)

    $borderPen = New-Object System.Drawing.Pen -ArgumentList ([System.Drawing.Color]::FromArgb(220, 255, 183, 77)), ([Math]::Max(1, $Size * 0.035))
    $graphics.DrawPath($borderPen, $path)

    $accentPen = New-Object System.Drawing.Pen -ArgumentList ([System.Drawing.Color]::FromArgb(255, 255, 132, 0)), ([Math]::Max(2, $Size * 0.08))
    $accentPen.StartCap = [System.Drawing.Drawing2D.LineCap]::Round
    $accentPen.EndCap = [System.Drawing.Drawing2D.LineCap]::Round
    $graphics.DrawLine($accentPen, $Size * 0.26, $Size * 0.72, $Size * 0.75, $Size * 0.27)

    $sparkBrush = New-Object System.Drawing.SolidBrush -ArgumentList ([System.Drawing.Color]::FromArgb(255, 255, 210, 120))
    $sparkRadius = [Math]::Max(2, $Size * 0.1)
    $graphics.FillEllipse($sparkBrush, $Size * 0.63, $Size * 0.16, $sparkRadius, $sparkRadius)

    $fontSize = $Size * 0.34
    $font = [System.Drawing.Font]::new("Segoe UI", [float]$fontSize, [System.Drawing.FontStyle]::Bold, [System.Drawing.GraphicsUnit]::Pixel)
    $textBrush = New-Object System.Drawing.SolidBrush -ArgumentList ([System.Drawing.Color]::FromArgb(255, 244, 246, 248))
    $format = New-Object System.Drawing.StringFormat
    $format.Alignment = [System.Drawing.StringAlignment]::Center
    $format.LineAlignment = [System.Drawing.StringAlignment]::Center

    $textRect = New-Object System.Drawing.RectangleF -ArgumentList ($Size * 0.11), ($Size * 0.18), ($Size * 0.52), ($Size * 0.52)
    $graphics.DrawString("PF", $font, $textBrush, $textRect, $format)

    $format.Dispose()
    $textBrush.Dispose()
    $font.Dispose()
    $sparkBrush.Dispose()
    $accentPen.Dispose()
    $borderPen.Dispose()
    $backgroundBrush.Dispose()
    $path.Dispose()
    $graphics.Dispose()

    return $bitmap
}

function Write-IcoFile {
    param(
        [string]$Path,
        [byte[][]]$ImageBytes,
        [int[]]$Sizes
    )

    $fileStream = [System.IO.File]::Create($Path)
    $writer = New-Object System.IO.BinaryWriter($fileStream)
    try {
        $writer.Write([UInt16]0)
        $writer.Write([UInt16]1)
        $writer.Write([UInt16]$ImageBytes.Count)

        $offset = 6 + ($ImageBytes.Count * 16)
        for ($index = 0; $index -lt $ImageBytes.Count; $index++) {
            $size = $Sizes[$index]
            $writer.Write([byte]([Math]::Min($size, 255) % 256))
            $writer.Write([byte]([Math]::Min($size, 255) % 256))
            $writer.Write([byte]0)
            $writer.Write([byte]0)
            $writer.Write([UInt16]1)
            $writer.Write([UInt16]32)
            $writer.Write([UInt32]$ImageBytes[$index].Length)
            $writer.Write([UInt32]$offset)
            $offset += $ImageBytes[$index].Length
        }

        foreach ($image in $ImageBytes) {
            $writer.Write($image)
        }
    }
    finally {
        $writer.Dispose()
        $fileStream.Dispose()
    }
}

$resolvedOutputDirectory = [System.IO.Path]::GetFullPath($OutputDirectory)
[System.IO.Directory]::CreateDirectory($resolvedOutputDirectory) | Out-Null

$sizes = @(16, 24, 32, 48, 64, 128, 256)
$iconBytes = New-Object System.Collections.Generic.List[byte[]]

foreach ($size in $sizes) {
    $bitmap = New-IconBitmap -Size $size
    try {
        $stream = New-Object System.IO.MemoryStream
        try {
            $bitmap.Save($stream, [System.Drawing.Imaging.ImageFormat]::Png)
            $iconBytes.Add($stream.ToArray())
        }
        finally {
            $stream.Dispose()
        }
    }
    finally {
        $bitmap.Dispose()
    }
}

$iconPath = Join-Path $resolvedOutputDirectory 'PromptForge.ico'
Write-IcoFile -Path $iconPath -ImageBytes $iconBytes.ToArray() -Sizes $sizes
Write-Host "Created $iconPath"
