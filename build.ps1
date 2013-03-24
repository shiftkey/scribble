function Get-ScriptDirectory
{
	$Invocation = (Get-Variable MyInvocation -Scope 1).Value
	Split-Path $Invocation.MyCommand.Path
}

$nuspec = Join-Path (Get-ScriptDirectory) src\scribble.nuspec
$basePath = Join-Path (Get-ScriptDirectory) src\package

$nuget = Join-Path (Get-ScriptDirectory) tools\NuGet.exe

. $nuget pack $nuspec -BasePath $basePath -OutputDir D:\Code\packages\ -NoPackageAnalysis
