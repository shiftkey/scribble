param($installPath, $toolsPath, $package, $project)

function FindRootFolder {
    # worst case scenario, we dump this in the same folder as the solution file
    $solution_path = $dte.Solution.FullName
    $solution_dir = (get-item $solution_path).Directory.FullName

    Write-Debug "Defaulting to $solution_dir if we can't find a source control folder"

    # best case scenario, we dump this in the root of the repository, which we get by looking for .git or .hg

    $root_dir = $null
    $current_dir = $toolsPath
    $depth_from_tool_path = 7 # how deep should we go?

    do {
	    $current_dir = (Get-Item $current_dir).Parent.FullName
        
        if ([IO.Directory]::Exists($current_dir) -eq $false)
        {
        	Write-Debug "We can't go further, bailing out"
            break;
        }

        if ([IO.Directory]::Exists((Join-Path -Path $current_dir ".git")))
        {
            Write-Debug "Found a .git folder at $current_dir!"
            $root_dir = $current_dir
        }

        if ([IO.Directory]::Exists((Join-Path -Path $current_dir ".hg")))
        {
            Write-Debug "Found a .hg folder at $current_dir!"
            $root_dir = $current_dir
        }
	
	    $depth_from_tool_path--
    } while ($depth_from_tool_path -gt 0 -and $root_dir -eq $null)

    if ($root_dir -ne $null)  {
    	Write-Debug "We found a source code directory, so we'll put the docs in the same location"
        return $root_dir
    } else { 
    	Write-Debug "No source code folder found, using solution directory"
        return $solution_dir
    }
}

function Setup-FolderStructure {
    Write-Debug "First time setup of documentation"
}

function Upgrade-FolderStructure {
    Write-Debug "Existing documentation folder already found"
}

# get the "project" root folder
$project_root = (FindRootFolder);

Write-Host "Adding docs folder into folder $project_root"

$docs_folder = Join-Path -Path $project_root "docs"
$configFile = Join-Path -Path $docs_folder "scribble.json"
$version = $package.Version.Version.ToString()

# if this is the first time we run the package
if ([IO.Directory]::Exists($docs_folder) -eq $false) {

    Setup-FolderStructure
    
    # create folder
	$templatePath = Join-Path -Path $toolsPath "template\*"

	# populate folder with template contents
    New-Item -ItemType directory -Path $docs_folder
	Copy-Item -Path $templatePath -Destination $docs_folder -Recurse | Out-Null

    # $port =  Get-Random -Minimum 30000 -Maximum 50000
    $port = 40000
    
	# dump the version and created port to the config file
    $properties = @{}
    $properties.Add('version',$version)
    $properties.Add('port',$port)
    ConvertTo-Json $properties | Out-File $configFile

    # TODO: overwrite template references to localhost:????? to localhost:40000

	# open the file in Visual Studio
    $index = Join-Path -Path $docs_folder "index.md"
	$dte.ItemOperations.OpenFile($index)

} else {  

	$installed_version = $null
    $port = 40000 

	if ([IO.File]::Exists($configFile) -eq $true) {
        $json = Get-Content $metafile | Out-String 
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

# TODO: hook in any other commands

# launch ze missiles
$launch_script = Join-Path -Path $toolsPath "_pretzel\Launch-Docs.ps1"
powershell -File $launch_script -DocsRoot $docs_folder -PortNumber $port