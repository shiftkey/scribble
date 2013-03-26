Scribble is a NuGet package to do the heavy lifting of creating and maintaining documentation in your projects.

Specifically it is intended to:

 - make documentation easy and fun to write
 - keep code and documentation in sync
 - take your documentation and export it

### Impatient? Go try it out!

**Note:** This is still a pre-release package. While I am still working on core components it may drown kittens. Approach with caution. Feedback is still welcome.

<div class="nuget-badge">
  <p><code>PM&gt; Install-Package scribble -Pre</code></p>
</div>

### Getting Started in 3 easy steps

 - Open your project in Visual Studio
 - Install the package from the Package Manager Console
 - There is no step 3

As part of the installation, Scribble will create the folder structure under `docs` at the root of your project (or repository if it can find a git/hg repository)

![files created](/scribble/images/folder-structure.png)

Scribble will also open the Markdown template for the index so you can start editing quickly

![edit the file](/scribble/images/edit-file.png)

In the background Scribble will open a tiny webserver to transform the Markdown files and other files into a website which you can view immediately in the browser

![preview the file](/scribble/images/view-site.png)

### What else is there?

Check the [docs folder](https://github.com/shiftkey/scribble/tree/master/docs/) for details about the concepts and ideas.
