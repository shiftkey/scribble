---
layout: page
---   

## [Proposal] Publish Documentation

It should be really easy to take your Markdown files and publish them somewhere.

What sort of publishing?

 - GitHub Pages - switch to gh-pages branch, replace content with generated HTML/CSS/JS, commit, push
 - Heroku - raw publishing to Heroku
 - WebDeploy - allows for syncing of content with an external server - requires config, permissions etc
 - FTP - replace content with generated HTML/CSS/JS (requires config, permissions, etc)
 - Zip - export the generated content to a specific file
 
What other platforms are of interest to you?

### GitHub Pages

Dependency: git
Configuration: 
 - remote git repository to push code to
 - HTTP: username/password permissions to repository (we don't want this in source control)
 - SSH: key (this won't be in source control either)

So at any point in time you can run:

    Publish-GitHubPages

Which will...

 - create a temporary clone of the repository (aiming to do *just* the gh-pages branch)
 - overwrite the `gh-pages` branch with the contents of the `docs` folder
 - change some config values based on how the site will be deployed
 - commit and push the changes back to your local clone
 - (optional) push the changes straight to GitHub

### Heroku  

Dependency: git
Configuration: 
 - remote git repository to push code to
 - HTTP: username/password permissions to repository (we don't want this in source control)
 - SSH: key (this won't be in source control either

This could be a simpler workflow than the above:

 - take the contents of the docs folder
 - commit it to a specific branch (convention: name it `jekyll`)
 - `git push heroku jekyll`

### WebDeploy (MSDeploy)

Dependency: MSDeploy client (licensing question: am I allowed to ship these binaries?)

Configuration: 
 - remote URL to send content to
 - username/password permissions to send files (we don't want this in source control)

TODO: sketch out API calls which would be necessary to authenitcate and publish folder containing site content

TODO: do we need/want to handle the first-time initialization of a site? is that even possible?

### FTP

Dependency: command-line FTP client (no preference, but Windows ships with [one](http://www.nsftools.com/tips/MSFTP.htm))

Configuration: 
 - remote URL to send content to
 - username/password permissions to send files (we don't want this in source control)

