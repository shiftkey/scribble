
function OpenFile {
	param ($File)

	Write-Host "Opening file $File in editor"
}

function GetPages($context) {
	$parameters = @{}
    
    if ($context.file) { $parameters.filter = $context.file }

    Write-Host "Found some text: " + $parameters.filter
}


function Delete-Page {
	param($File)

	Write-Host "Deleting file from path $File"
}

function Edit-Page {
	param($File)

	OpenFile $File	
}

function Add-Page {
	param($Name)

	Write-Host "Creating file $File in folder"
	OpenFile $File
}

Register-TabExpansion 'Delete-Page' 
@{ 'File' = { 
	param($context)
    GetPages($context) }
}

Register-TabExpansion 'Edit-Page' 
@{ 'File' = { 	
	param($context)
    GetPages($context) } 
}

Export-ModuleMember Delete-Page -Alias Remove-Page
Export-ModuleMember Edit-Page -Alias Open-Page
Export-ModuleMember Add-Page -Alias Create-Page