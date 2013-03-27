function Sync-Docs {
    $root = "D:\Code\github\shiftkey\scribble\src\Scribble.CodeSnippets" 
    $assemblyPath = Join-Path $root "Scribble.CodeSnippets\bin\Debug\Scribble.CodeSnippets.dll"

    $codeFolder = "D:\Code\github\shiftkey\Newtonsoft.Json\Src\"
    $docsFolder = "D:\Code\github\shiftkey\Newtonsoft.Json\docs\"

    [void][Reflection.Assembly]::LoadFile($assemblyPath)  

    # parse and merge files
    [Scribble.CodeSnippets.Importer]::Update($codeFolder, @("*.cs"), $docsFolder)
}

Sync-Docs