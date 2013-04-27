# [Proposal] Publish Documentation

It should be really easy to take your Markdown files and publish them somewhere.

What sort of publishing?

 - GitHub Pages - switch to gh-pages branch, replace content with generated HTML/CSS/JS, commit, push
 - Heroku - raw publishing to Heroku
 - WebDeploy - allows for syncing of content with an external server - requires config, permissions etc
 - FTP - replace content with generated HTML/CSS/JS (requires config, permissions, etc)
 - Zip - export the generated content to a specific file
 
What other platforms are of interest to you?

## GitHub Pages

Dependency: git
Configuration: 
 - remote git repository to push code to
 - HTTP: username/password permissions to repository (we don't want this in source control)
 - SSH: key (this won't be in source control either)

So I just spiked this pseduo-script together (the git bits work, the rest I'm less sure of).

NOTE: assumes git is installed on the dev's machine and is in the PATH
TODO: make git.exe available within tools and ensure dev does not need to have in PATH

	mkdir temp && cd temp
	# create a stub repo
	git init
	# we could use the local repo here
	git remote add upstream https://github.com/shiftkey/scribble.git    
	# just fetch changes for one branch
	git fetch upstream +gh-pages
	# switch to that branch
	git checkout gh-pages
	# clobber it all
	rm -rf .
	# dump in new site contents
	cp ../_site/ . -r
	# add all the things
	git add .
	git add -u .
	# new commit
	git commit -m "updated site"
	# to infinity and beyond
	git push upstream gh-pages
	# tidy up
	cd ..
	rm -rf temp

## Heroku  

Dependency: git
Configuration: 
 - remote git repository to push code to
 - HTTP: username/password permissions to repository (we don't want this in source control)
 - SSH: key (this won't be in source control either

This could be a simpler workflow than the above:

 - take the contents of the docs folder
 - commit it to a specific branch (convention: name it `jekyll`)
 - `git push heroku jekyll`

## WebDeploy (MSDeploy)

Dependency: MSDeploy client (licensing question: am I allowed to ship these binaries?)

Configuration: 
 - remote URL to send content to
 - username/password permissions to send files (we don't want this in source control)

TODO: sketch out API calls which would be necessary to authenitcate and publish folder containing site content

TODO: do we need/want to handle the first-time initialization of a site? is that even possible?

## FTP

Dependency: command-line FTP client (no preference, but Windows ships with [one](http://www.nsftools.com/tips/MSFTP.htm))

Configuration: 
 - remote URL to send content to
 - username/password permissions to send files (we don't want this in source control)

