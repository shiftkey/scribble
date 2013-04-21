param($rootDir, $toolsPath)

function Update-Snippets {
	param(
		$Extensions = @("*.cs"),
		[switch] $Debug
	)
    
    $codeFolder = $rootDir
    $docsFolder = Join-Path $rootDir "docs"

    $result = [Scribble.CodeSnippets.CodeImporter]::Update($codeFolder, $Extensions, $docsFolder)

	$ticks = $result.ElapsedMilliseconds
	Write-Host "Update took $ticks ms"

    if ($result.Completed -eq $false) {
    	Write-Error "Oops, something went wrong with the update"
    }

    foreach ($error in $result.Errors) { 
    	$message = $error.Message
    	$file = $error.File
    	$line = $error.LineNumber
    	Write-Error "$message"
    	Write-Error "File: $file - Line Number: $line"
    }

    if ($Debug) {
		foreach ($warning in $result.Warnings) {
			$message = $warning.Message
			$file = $warning.File
			$line = $warning.LineNumber
			Write-Host "$message"
			Write-Host "File: $file - Line Number: $line"
		}
    }
}

Export-ModuleMember Update-Snippets 