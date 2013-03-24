Param(
  [string]$DocsRoot,
  [string]$PortNumber
)

Set-StrictMode -Version Latest

$ErrorActionPreference = "Stop"

$ExistingListener = (& netstat -anop tcp) | where { $_ -match "0.0.0.0:$PortNumber\s+0.0.0.0:0\s+LISTENING" }
if ($ExistingListener -ne $null)
{
	[void]($ExistingListener -match "\d+$")
	$ProcessIdOfCurrentlyRunningPretzelProcess = $matches[0]
	"Pretzel is already running under PID $ProcessIdOfCurrentlyRunningPretzelProcess"
	
	$Process = [System.Diagnostics.Process]::GetProcessById($ProcessIdOfCurrentlyRunningPretzelProcess)
	$Process.Kill()
	"Pretzel $ProcessIdOfCurrentlyRunningPretzelProcess killed"
}

$SiteCache = Join-Path $DocsRoot _site
if (Test-Path $SiteCache) {
	"Clearing old content"
	Remove-Item $SiteCache -Recurse -Force
}

[void](New-Item $SiteCache -ItemType Directory)
$SiteCacheDirectoryInfo = New-Object System.IO.DirectoryInfo $SiteCache
$SiteCacheDirectoryInfo.Attributes = [System.IO.FileAttributes] "Directory, Hidden"

"Launching Pretzel"
Remove-Item $Env:TEMP\*.* -Include Pretzel.*
Copy-Item $PSScriptRoot\*.* -Include Pretzel.* $Env:TEMP -Force
$PretzelExe = Join-Path $Env:TEMP Pretzel.exe

$result = (& $PretzelExe bake -debug --directory $DocsRoot -WindowStyle Normal)
if ($result -like "Fail*") {
	$result
	throw "Pretzel bake failed"
}

[void](Start-Process $PretzelExe "taste -debug --directory $DocsRoot --port $PortNumber")