param([switch]$PushToRemote)

$scriptpath = $MyInvocation.MyCommand.Path
$toolsPath = Split-Path $scriptpath
$root_dir = Split-Path $toolsPath
Write-host "My directory is $root_dir"

$assemblyPath = Join-Path $toolsPath "libgit2sharp\LibGit2Sharp.dll"
[void][Reflection.Assembly]::LoadFile($assemblyPath)

function Update-GithubPages($current_repository, $base_url) {

	$currentLocation = (Get-Location)

	$tempPath = [System.IO.Path]::GetTempPath()
	$guid = [guid]::NewGuid()
	$repoPath = Join-Path $tempPath $guid

	"Creating temporary folder at $repoPath"

	New-Item -Type directory $repoPath | Out-Null

	"Setting up clone of gh-pages repository"

	Set-Location -Path $repoPath

    # TODO: can we just recreate the gh-pages branch without needing a full clone?
	$repo = [LibGit2Sharp.Repository]::Clone($current_repository, $repoPath)
	$branch = $repo.Checkout("origin/gh-pages", [LibGit2Sharp.CheckoutOptions]::None, $null)
	"Replacing contents with generated content"

	Remove-Item * -Exclude .git -Recurse -Force
	$site_folder = Join-Path $current_repository "\docs\*"
	Copy-Item -Path $site_folder -Exclude "_site" -Recurse -Destination .

    # update template references to GitHub URL
    $template = Get-Content "_config.yml"
    $template -replace "baseurl: /", "baseurl: $base_url" | Out-File "_config.yml" -encoding "ASCII"

    $repo.Index.Stage("*");
    $args = @("scribble", "scribble@nowhere.com", [System.DateTimeOffset]::Now)
    $signature = New-Object -Type LibGit2Sharp.Signature -ArgumentList $args
    $repo.Commit("updating site from scribble generated content", $signature, $signature) | Out-Null
       
    Write-Host $branch.Remote.Name
    # TODO: we need this to work against a proper remote
    $repo.Network.Push($branch.Remote, $branch.CanonicalName, $null)

	$repo.Dispose()

	Set-Location -Path $currentLocation

	"Removing temporary folder at $repoPath"
	Remove-Item $repoPath -Recurse -Force | Out-Null
}

Update-GithubPages "D:\Code\github\shiftkey\scribble\" "/scribble/"

if ($PushToRemote) { 
	"Pushing to GitHub now"
	# TODO: push this
    # . git push origin gh-pages --force
}
