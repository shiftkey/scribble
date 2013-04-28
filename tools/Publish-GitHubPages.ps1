param([switch]$PushToRemote)


function Update-GithubPages($current_repository, $base_url, $remote_url) {

	$currentLocation = (Get-Location)

	$tempPath = [System.IO.Path]::GetTempPath()
	$guid = [guid]::NewGuid()
	$repoPath = Join-Path $tempPath $guid

	"Creating temporary folder at $repoPath"

	New-Item -Type directory $repoPath | Out-Null

	"Setting up clone of gh-pages repository"

	Set-Location -Path $repoPath

	git clone -b gh-pages -o upstream $current_repository .

	"Replacing contents with generated content"

	Remove-Item * -Exclude .git -Recurse -Force
	$site_folder = Join-Path $current_repository "\docs\*"
	Copy-Item -Path $site_folder -Exclude "_site" -Recurse -Destination .

    # update template references to GitHub URL
    $template = Get-Content "_config.yml"
    $template -replace "baseurl: /", "baseurl: $base_url" | out-file "_config.yml"

	. git add .
	. git add -u .
	. git commit -m "updating site from scribble generated content"
	. git push upstream gh-pages

	Set-Location -Path $currentLocation

	"Removing temporary folder at $repoPath"
	Remove-Item $repoPath -Recurse -Force
}

Update-GithubPages "D:\Code\github\shiftkey\scribble\" "/scribble" "http://shiftkey.github.io/scribble/"

if ($PushToRemote) { 
    . git push origin gh-pages
}
