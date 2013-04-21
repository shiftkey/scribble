$portNumber = '42428'

function Get-ScriptDirectory
{
	$Invocation = (Get-Variable MyInvocation -Scope 1).Value
	Split-Path $Invocation.MyCommand.Path
}

$script_root = Get-Item (Get-ScriptDirectory)
$project_root = $script_root.Parent.FullName
$docs_folder = Join-Path $project_root "docs"
$toolsPath = Join-Path $project_root "src\package\tools"

Import-Module (Join-Path $toolsPath Start-Preview.psm1) -ArgumentList @($project_root, $docs_folder, $portNumber)

Start-Preview($true)