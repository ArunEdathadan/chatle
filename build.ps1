$ErrorActionPreference = "Stop"

Write-Host "Powershell version: " $PSVersionTable.PSVersion

function DownloadWithRetry([string] $url, [string] $downloadLocation, [int] $retries) 
{
    while($true)
    {
        try
        {
            Invoke-WebRequest $url -OutFile $downloadLocation
            break
        }
        catch
        {
            $exceptionMessage = $_.Exception.Message
            Write-Host "Failed to download '$url': $exceptionMessage"
            if ($retries -gt 0) {
                $retries--
                Write-Host "Waiting 10 seconds before retrying. Retries left: $retries"
                Start-Sleep -Seconds 10

            }
            else 
            {
                $exception = $_.Exception
                throw $exception
            }
        }
    }
}

cd $PSScriptRoot

$repoFolder = $PSScriptRoot
$env:REPO_FOLDER = $repoFolder

$koreBuildZip="https://github.com/aspnet/KoreBuild/archive/dev.zip"
if ($env:KOREBUILD_ZIP)
{
    $koreBuildZip=$env:KOREBUILD_ZIP
}

$buildFolder = ".build"
$buildFile="$buildFolder\KoreBuild.ps1"

if (!(Test-Path $buildFolder)) {
    Write-Host "Downloading KoreBuild from $koreBuildZip"    
    
    $tempFolder=$env:TEMP + "\KoreBuild-" + [guid]::NewGuid()
    New-Item -Path "$tempFolder" -Type directory | Out-Null

    $localZipFile="$tempFolder\korebuild.zip"
    
    DownloadWithRetry -url $koreBuildZip -downloadLocation $localZipFile -retries 6

    Add-Type -AssemblyName System.IO.Compression.FileSystem
    [System.IO.Compression.ZipFile]::ExtractToDirectory($localZipFile, $tempFolder)
    
    New-Item -Path "$buildFolder" -Type directory | Out-Null
    copy-item "$tempFolder\**\build\*" $buildFolder -Recurse

    # Cleanup
    if (Test-Path $tempFolder) {
        Remove-Item -Recurse -Force $tempFolder
    }
}

# We still nuget because dotnet doesn't have support for pushing packages
Invoke-WebRequest "https://dist.nuget.org/win-x86-commandline/v3.5.0-beta2/NuGet.exe" -OutFile "$buildFolder/nuget.exe"

&"$buildFolder\NuGet.exe" install GitVersion.CommandLine -ExcludeVersion -Source https://www.nuget.org/api/v2/ -Out packages
&"$buildFolder\NuGet.exe" install OpenCover -ExcludeVersion -Source https://www.nuget.org/api/v2/ -Version 4.6.166 -Out packages
&"$buildFolder\NuGet.exe" install coveralls.io -ExcludeVersion -Source https://www.nuget.org/api/v2/  -Out packages
&"$buildFolder\NuGet.exe" install ReportGenerator -ExcludeVersion -Source https://www.nuget.org/api/v2/ -Out packages
&"$buildFolder\NuGet.exe" install Chutzpah -ExcludeVersion -Source https://www.nuget.org/api/v2/ -Out packages


&"$buildFile" $args