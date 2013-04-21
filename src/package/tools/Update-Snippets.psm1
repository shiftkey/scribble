param($rootDir, $toolsPath)

function Update-Snippets {
    
    $assemblyPath = Join-Path $toolsPath "codesnippets\Scribble.CodeSnippets.dll"

    $codeFolder = $rootDir
    $docsFolder = Join-Path $rootDir "docs"

    [void][Reflection.Assembly]::LoadFile($assemblyPath)  

    [Scribble.CodeSnippets.CodeImporter]::Update($codeFolder, @("*.cs"), $docsFolder)
}

Export-ModuleMember Update-Snippets 