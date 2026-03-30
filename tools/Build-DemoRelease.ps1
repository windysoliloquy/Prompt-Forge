[CmdletBinding()]
param(
    [string]$Configuration = 'Release',
    [string]$RuntimeIdentifier = 'win-x64',
    [string]$PublishProfile = 'PromptForgeDemo',
    [string]$ReadyOutputDir = 'C:\Users\windy\OneDrive\Desktop\ready',
    [string]$InnoSetupCompilerPath,
    [switch]$SkipPublish,
    [switch]$SkipInstaller
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$projectRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$appProject = Join-Path $projectRoot 'PromptForge.App\PromptForge.App.csproj'
$iconScript = Join-Path $projectRoot 'tools\New-PromptForgeIcon.ps1'
$installerScript = Join-Path $projectRoot 'tools\installer\PromptForgeDemo.iss'
$releaseReadmeSource = Join-Path $projectRoot 'docs\PromptForge-Demo-README.txt'
$publishDir = Join-Path $projectRoot 'artifacts\demo\publish'
$installerOutputDir = Join-Path $projectRoot 'artifacts\demo\installer'
$readyDir = [System.IO.Path]::GetFullPath($ReadyOutputDir)
$readyPublishDir = Join-Path $readyDir 'PromptForge-Demo'
$legacyReadyPublishDir = Join-Path $readyDir 'PromptForge-Demo-Publish'
$readyShortcutPath = Join-Path $readyDir 'PromptForge Demo.lnk'
$readyReadmePath = Join-Path $readyDir 'README.txt'

function Remove-DirectoryContents {
    param([string]$Path)

    if (Test-Path -LiteralPath $Path) {
        Get-ChildItem -LiteralPath $Path -Force | Remove-Item -Recurse -Force
    }
    else {
        New-Item -ItemType Directory -Path $Path | Out-Null
    }
}

function Resolve-InnoSetupCompiler {
    param([string]$RequestedPath)

    if ($RequestedPath) {
        if (-not (Test-Path -LiteralPath $RequestedPath)) {
            throw "Inno Setup compiler was not found at '$RequestedPath'."
        }

        return [System.IO.Path]::GetFullPath($RequestedPath)
    }

    $candidates = @(
        'C:\Users\windy\AppData\Local\Programs\Inno Setup 6\ISCC.exe',
        'C:\Program Files (x86)\Inno Setup 6\ISCC.exe',
        'C:\Program Files\Inno Setup 6\ISCC.exe'
    )

    foreach ($candidate in $candidates) {
        if (Test-Path -LiteralPath $candidate) {
            return $candidate
        }
    }

    return $null
}

function New-ShortcutFile {
    param(
        [string]$ShortcutPath,
        [string]$TargetPath,
        [string]$WorkingDirectory,
        [string]$IconLocation
    )

    $shell = New-Object -ComObject WScript.Shell
    try {
        $shortcut = $shell.CreateShortcut($ShortcutPath)
        $shortcut.TargetPath = $TargetPath
        $shortcut.WorkingDirectory = $WorkingDirectory
        $shortcut.IconLocation = $IconLocation
        $shortcut.Save()
    }
    finally {
        [System.Runtime.InteropServices.Marshal]::ReleaseComObject($shell) | Out-Null
    }
}

function Remove-MatchingFiles {
    param(
        [string]$RootPath,
        [string]$Filter
    )

    if (Test-Path -LiteralPath $RootPath) {
        Get-ChildItem -LiteralPath $RootPath -Recurse -File -Filter $Filter | Remove-Item -Force
    }
}

if (-not (Test-Path -LiteralPath $appProject)) {
    throw "PromptForge.App project was not found at '$appProject'."
}

if (-not (Test-Path -LiteralPath (Join-Path $projectRoot 'PromptForge.App\Assets\PromptForge.ico'))) {
    & $iconScript
}

New-Item -ItemType Directory -Force -Path $publishDir, $installerOutputDir, $readyDir | Out-Null

if (-not $SkipPublish) {
    Remove-DirectoryContents -Path $publishDir

    & dotnet publish $appProject `
        -c $Configuration `
        -p:PublishProfile=$PublishProfile `
        -p:PromptForgeDemoMode=true `
        -p:RuntimeIdentifier=$RuntimeIdentifier `
        -p:SelfContained=true `
        -p:PublishSingleFile=false

    if ($LASTEXITCODE -ne 0) {
        throw "dotnet publish failed."
    }
}

$publishedExe = Join-Path $publishDir 'PromptForge.App.exe'
if (-not (Test-Path -LiteralPath $publishedExe)) {
    throw "Published demo executable was not found at '$publishedExe'."
}

Remove-MatchingFiles -RootPath $publishDir -Filter '*.pdb'

if (Test-Path -LiteralPath $legacyReadyPublishDir) {
    Remove-Item -LiteralPath $legacyReadyPublishDir -Recurse -Force
}

Remove-DirectoryContents -Path $readyPublishDir
Get-ChildItem -LiteralPath $publishDir -Force | Copy-Item -Destination $readyPublishDir -Recurse -Force

$readyExePath = Join-Path $readyPublishDir 'PromptForge.App.exe'
if (-not (Test-Path -LiteralPath $readyExePath)) {
    throw "Ready demo executable was not found at '$readyExePath' after copying the publish output."
}

New-ShortcutFile `
    -ShortcutPath $readyShortcutPath `
    -TargetPath $readyExePath `
    -WorkingDirectory $readyPublishDir `
    -IconLocation $readyExePath

if (Test-Path -LiteralPath $releaseReadmeSource) {
    Copy-Item -LiteralPath $releaseReadmeSource -Destination $readyReadmePath -Force
}

if (-not $SkipInstaller) {
    $iscc = Resolve-InnoSetupCompiler -RequestedPath $InnoSetupCompilerPath
    if ($iscc) {
        Remove-DirectoryContents -Path $installerOutputDir

        $appVersion = [System.Diagnostics.FileVersionInfo]::GetVersionInfo($publishedExe).FileVersion
        if ([string]::IsNullOrWhiteSpace($appVersion)) {
            $appVersion = '1.0.0-demo'
        }

        & $iscc `
            "/DSourceDir=$publishDir" `
            "/DOutputDir=$installerOutputDir" `
            "/DSetupBaseName=PromptForge-Demo-Setup" `
            "/DAppVersion=$appVersion" `
            "/DAppExeName=PromptForge.App.exe" `
            "/DSetupIconFile=$(Join-Path $projectRoot 'PromptForge.App\Assets\PromptForge.ico')" `
            $installerScript

        if ($LASTEXITCODE -ne 0) {
            throw "Inno Setup compilation failed."
        }

        $installerPath = Join-Path $installerOutputDir 'PromptForge-Demo-Setup.exe'
        if (-not (Test-Path -LiteralPath $installerPath)) {
            throw "Expected installer was not produced at '$installerPath'."
        }

        Copy-Item -LiteralPath $installerPath -Destination (Join-Path $readyDir 'PromptForge-Demo-Setup.exe') -Force
        Write-Host "Installer copied to $(Join-Path $readyDir 'PromptForge-Demo-Setup.exe')"
    }
    else {
        Write-Warning "Inno Setup was not found. Published demo files were copied to '$readyPublishDir', but no installer was generated."
    }
}

Write-Host "Demo publish folder: $publishDir"
Write-Host "Ready publish copy: $readyPublishDir"
Write-Host "Ready shortcut: $readyShortcutPath"
Write-Host "Ready readme: $readyReadmePath"
Write-Host "Installer output folder: $installerOutputDir"
