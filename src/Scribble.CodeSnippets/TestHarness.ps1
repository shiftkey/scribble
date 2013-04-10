function Update-Snippets {
    $root = "D:\Code\github\shiftkey\scribble\" 
    $assemblyPath = Join-Path $root "src\package\tools\codesnippets\Scribble.CodeSnippets.dll"

    $codeFolder = "D:\Code\github\shiftkey\Newtonsoft.Json\Src\"
    $docsFolder = "D:\Code\github\shiftkey\Newtonsoft.Json\docs\"

    [void][Reflection.Assembly]::LoadFile($assemblyPath)  

    # parse and merge files
    [Scribble.CodeSnippets.CodeImporter]::Update($codeFolder, @("*.cs"), $docsFolder)
}

Update-Snippets 