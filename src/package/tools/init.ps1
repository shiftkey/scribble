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

# get the "project" root folder
$project_root = (FindRootFolder);

Write-Host "Installing docs folder inside $project_root"

$docs_folder = Join-Path -Path $project_root "docs"
$metafile = Join-Path -Path $docs_folder ".duchess"
$version = $package.Version.Version.ToString()

# if this is the first time we run the package
if ([IO.Directory]::Exists($docs_folder) -eq $false) {
	# create folder
	$templatePath = Join-Path -Path $toolsPath "template\*"

	# populate folder with template contents
	Copy-Item -Path $templatePath -Destination $docs_folder -Recurse

	# dump the package version to a metafile
	$version | Out-File $metafile

	# open the file in Visual Studio
	$index = Join-Path -Path $docs_folder "index.md"
	$dte.ItemOperations.OpenFile($index)
}
else {
	# if we have a different version installed, this is an upgrade
	$installed_version = $null

	if ([IO.File]::Exists($metafile) -eq $false) {
		$installed_version = "i have no idea"
	} else {
		$installed_version = Get-Content $metafile | Out-String
	}

	if ($installed_version -ne $package) {
		# update the version 
		Remove-Item $metafile
		$version | Out-File $metafile	
	}
}

# TODO: hook in any other commands