param($installPath, $toolsPath, $package, $project)

function FindRootFolder {
    # worst case scenario, we dump this in the same folder as the solution file
    $solution_path = $dte.Solution.FullName
    $solution_dir = (get-item $solution_path).Directory.FullName

    Write-Host "Defaulting to $solution_dir if we can't find a source control folder"

    # best case scenario, we dump this in the root of the repository, which we get by looking for .git or .hg

    $root_dir = $null
    $current_dir = $toolsPath
    $depth_from_tool_path = 7 # how deep should we go?

    do {
	    $current_dir = (Get-Item $current_dir).Parent.FullName
        
        if ([IO.Directory]::Exists($current_dir) -eq $false)
        {
        	Write-Host "We can't go further, bailing out"
            break;
        }

        if ([IO.Directory]::Exists((Join-Path -Path $current_dir ".git")))
        {
            Write-Host "Found a .git folder at $current_dir!"
            $root_dir = $current_dir
        }

        if ([IO.Directory]::Exists((Join-Path -Path $current_dir ".hg")))
        {
            Write-Host "Found a .hg folder at $current_dir!"
            $root_dir = $current_dir
        }
	
	    $depth_from_tool_path--
    } while ($depth_from_tool_path -gt 0 -and $root_dir -eq $null)

    if ($root_dir -ne $null)  {
    	Write-Host "We found a source code directory, so we'll put the docs in the same location"
        return $root_dir
    } else { 
    	Write-Host "No source code folder found, using solution directory"
        return $solution_dir
    }
}

function Setup-FolderStructure {
    Write-Host "First time setup of documentation"
}

function Upgrade-FolderStructure {
    Write-Host "Existing documentation folder already found"
}

# get the "project" root folder
$project_root = (FindRootFolder);

Write-Host "Installing docs folder inside $project_root"

$docs_folder = Join-Path -Path $project_root "docs"
$metafile = Join-Path -Path $docs_folder ".duchess"
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
    
	# dump the package version to a metafile
    '{ "version" : "$version", "port" : "$port" }' | Out-File $metafile

    # TODO: overwrite template references to localhost:????? to localhost:40000

	# open the file in Visual Studio
    $index = Join-Path -Path $docs_folder "index.md"
	$dte.ItemOperations.OpenFile($index)

} else {  

    # TODO: metafile now is a JSON file, what's an easy way to parse JSON from powershell?
	$installed_version = $null
	if ([IO.File]::Exists($metafile) -eq $true) {
		$installed_version = Get-Content $metafile | Out-String
    }

    $port = 40000 # TODO: read this from metadata file

	if ($installed_version -ne $package) {
        
        Upgrade-FolderStructure

		# update the version stored in the docs folder
        if ([IO.File]::Exists($metafile) -eq $true) {
            Remove-Item $metafile
        }
		$version | Out-File $metafile	
	} else {
        Write-Host "No changes necessary, you've got the latest code"
    }
}

# TODO: hook in any other commands

# launch ze missiles
$launch_script = Join-Path -Path $toolsPath "_pretzel\Launch-Docs.ps1"
powershell -File $launch_script -DocsRoot $docs_folder -PortNumber $port