function Get-ScriptDirectory
{
	$Invocation = (Get-Variable MyInvocation -Scope 1).Value
	Split-Path $Invocation.MyCommand.Path
}

$devFolder = "D:\packages"
$devPackages = Get-ChildItem $devFolder | Where {$_.Name -like "scribble*" }

if ($devPackages.length -eq 0) {
	$Version = "0.4.3.1000"
} else {
	$lastPackage = $devPackages[-1].Name
	$groups = ([regex]"\d+").matches($lastPackage).groups
	$build_number = $groups[-1].value
	$new_build_number = 1 + [int]$build_number

	$Version = $lastPackage -replace $build_number, $new_build_number `
						    -replace "scribble.", "" `
						    -replace "-pre.nupkg", ""
}


"Using new version number $Version"

powershell -File build.ps1 -Version $Version

$nuspec = Join-Path (Get-ScriptDirectory) src\scribble.nuspec
$basePath = Join-Path (Get-ScriptDirectory) src\package

$nuget = Join-Path (Get-ScriptDirectory) tools\NuGet.exe

. $nuget pack $nuspec -BasePath $basePath -Version "$Version-pre" -OutputDir $devFolder -NoPackageAnalysis
