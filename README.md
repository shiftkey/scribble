Scribble
=======

Scribble is a NuGet package to do the heavy lifting of creating and maintaining documentation in your projects.

Check the [docs folder](https://github.com/shiftkey/scribble/tree/master/docs/) for details about the concepts and ideas.

Specifically it is intended to:

 - make documentation easy and fun to write
 - keep code and documentation in sync
 - take your documentation and export it 

### Getting Started in 3 easy steps

 - Open your project in Visual Studio
 - `Install-Package scribble -Pre` from the Package Manager Console
 - There is no step 3

As part of the installation, Scribble will create the folder structure under `docs` in your solution folder (or repository root if it detects a git/hg repository) and open the site in your default browser.

![](https://raw.github.com/shiftkey/scribble/gh-pages/images/introduction/folder-structure.png)

Scribble will also open the Markdown template for the index so you can start editing quickly

![screenshot of docs folder](https://raw.github.com/shiftkey/scribble/gh-pages/images/introduction/edit-file.png)

In the background Scribble will open a tiny webserver to transform the Markdown files and other files into a website which you can view immediately in the browser

![screenshot of browser](https://raw.github.com/shiftkey/scribble/gh-pages/images/introduction/view-site.png)

When you open the solution after subsequent usages, the previewer will startup again so you can keep your documentation close.

### Features

#### Markdown everywhere

#### Code Snippet detection
