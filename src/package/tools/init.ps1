param($installPath, $toolsPath, $package, $project)

Import-Module (Join-Path $toolsPath Resolve-RootFolder.psm1)

function Setup-FolderStructure {
    Write-Debug "First time setup of documentation"
}

function Upgrade-FolderStructure {
    Write-Debug "Existing documentation folder already found"
}

$project_root = Resolve-RootFolder($toolsPath);

$docs_folder = Join-Path -Path $project_root "docs"
$configFile = Join-Path -Path $docs_folder "scribble.json"
$version = $package.ToString()

$startBrowser = $false

$directory_exists = [IO.Directory]::Exists($docs_folder)
$config_exists = [IO.File]::Exists($configFile)

# if this is the first time we run the package
if ($config_exists -eq $false) {

    Write-Host "Adding docs folder into folder $project_root"
    Setup-FolderStructure
    
    # create folder
	$templatePath = Join-Path -Path $toolsPath "template\*"

	# populate folder with template contents
    if ($directory_exists -eq $false) {
        New-Item -ItemType directory -Path $docs_folder
    }
	Copy-Item -Path $templatePath -Destination $docs_folder -Recurse | Out-Null

    $port = Get-Random -Minimum 30000 -Maximum 50000
    
	# dump the version and created port to the config file
    $properties = @{}
    $properties.Add('version',$version)
    $properties.Add('port',$port)
    ConvertTo-Json $properties | Out-File $configFile

    # TODO: overwrite template references to localhost:????? to localhost:40000

	# open the file in Visual Studio
    $index = Join-Path -Path $docs_folder "index.md"
	$dte.ItemOperations.OpenFile($index)

    $startBrowser = $true

    $dte.ItemOperations.Navigate("http://localhost:$port/")

} else {  

    Write-Host "Checking version of Scribble found locally"

	$installed_version = $null
    $port = 40000 

	if ([IO.File]::Exists($configFile) -eq $true) {
        $json = Get-Content $configFile | Out-String 
        $properties = ConvertFrom-Json $json
		$installed_version = $properties.version
        $port = $properties.port
    }

	if ($installed_version -ne $package) {
        
        Upgrade-FolderStructure

		# update the version stored in configuration
        if ([IO.File]::Exists($configFile) -eq $true) {
            Remove-Item $configFile
        }

        $properties = @{}
        $properties.Add('version',$version)
        $properties.Add('port',$port)
        ConvertTo-Json $properties | Out-File $configFile
	} else {
        Write-Debug "No changes necessary, you've got the latest code"
    }
}

$assemblyPath = Join-Path $toolsPath "codesnippets\Scribble.CodeSnippets.dll"
[void][Reflection.Assembly]::LoadFile($assemblyPath)  

Import-Module (Join-Path $toolsPath Update-Snippets.psm1) -ArgumentList @($project_root, $toolsPath)
Import-Module (Join-Path $toolsPath Start-Preview.psm1) -ArgumentList @($project_root, $docs_folder, $port)

if ($startBrowser) {
    Start-Preview
}