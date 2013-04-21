function Get-ScriptDirectory
{
	$Invocation = (Get-Variable MyInvocation -Scope 1).Value
	Split-Path $Invocation.MyCommand.Path
}

$Version = "0.4.3.1000"

powershell -File build.ps1 -Version $Version

$nuspec = Join-Path (Get-ScriptDirectory) src\scribble.nuspec
$basePath = Join-Path (Get-ScriptDirectory) src\package

$nuget = Join-Path (Get-ScriptDirectory) tools\NuGet.exe

. $nuget pack $nuspec -BasePath $basePath -Version "$Version-pre" -OutputDir D:\Code\packages\ -NoPackageAnalysis
