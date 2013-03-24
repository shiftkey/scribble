function Resolve-RootFolder ($start_dir) {

	# worst case scenario, we dump this in the same folder as the solution file
    $solution_path = $dte.Solution.FullName
    $solution_dir = (get-item $solution_path).Directory.FullName

    Write-Debug "Defaulting to $solution_dir if we can't find a source control folder"

    # best case scenario, we dump this in the root of the repository, which we get by looking for .git or .hg

    $root_dir = $null
    $current_dir = $start_dir
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