function Resolve-RootFolder ($start_dir) {

	# worst case scenario, we dump this in the same folder as the solution file
    $solution_path = $dte.Solution.FullName
    $solution_dir = (get-item $solution_path).Directory.FullName

    Write-Debug "Defaulting to $solution_dir if we can't find a source control folder"

    $current_dir = $start_dir
    $root_dir  = [LibGit2Sharp.Repository]::Discover($current_dir);
  
    if ($root_dir -ne $null)  {
    	Write-Debug "We found a source code directory, so we'll put the docs in the same location"
        return  (get-item -Force $root_dir).Parent.FullName # lol hidden folder
    } else { 
    	Write-Debug "No source code folder found, using solution directory"
        return $solution_dir
    }

} 