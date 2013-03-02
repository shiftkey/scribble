param($installPath, $toolsPath, $package, $project)

function FindRootFolder {

    # worst case scenario, we dump this in the same folder as the solution file
    $solution_path = $dte.Solution.FullName
    $solution_dir = (get-item $solution_path).Directory.FullName

    Write-Host "Defaulting to $solution_dir if we can't find a source control folder"

    # best case scenario, we dump this in the root of the repository, which we get by looking for .git or .hg

    $root_dir = $null
    $current_dir = $toolsPath
    $depth_from_tool_path = 5 # how deep should we go?

    do {
	    $current_dir = $current_dir.parent
        $parent_dir = $current_dir.FullName

        if ([IO.Directory]::Exists($parent_dir) -eq $false)
        {
        	Write-Host "We've gone too high, bailing out"
            break;
        }

        if ([IO.Directory]::Exists((Join-Path $parent_dir ".git")))
        {
            Write-Host "Found a .git folder at $parent_dir!"
            $root_dir = $parent_dir
        }

        if ([IO.Directory]::Exists((Join-Path $parent_dir ".hg")))
        {
            Write-Host "Found a .hg folder at $parent_dir!"
            $root_dir = $parent_dir
        }
	
	    $depth_from_tool_path--
    } while ($depth_from_tool_path -gt 0 -and $root_dir -eq $null)

    if ($root_dir -ne $null)  {
        return $root_dir
    } else { 
    	Write-Host "No source code folder found, using solution directory"
        return $solution_dir
    }
}

# get the "project" root folder
$project_root = (FindRootFolder);
$docs_folder = Join-Path -Path $project_root "docs"

$templatePath = Join-Path -Path $toolsPath "template\*"

# populate folder with template contents
Copy-Item -Path $templatePath -Destination $docs_folder -Recurse -Force

# open the file in Visual Studio
$index = Join-Path -Path $docs_folder "index.md"
$dte.ItemOperations.OpenFile($index)