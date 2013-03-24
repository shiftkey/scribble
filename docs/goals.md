# Goals

## Easy to get started

One of the big reasons why Sandcastle has such a bad name is because it is really hard to setup (and automate). XML documentation is another common thing for many codebases but quite often this is more for Intellisense than for end-user documentation.

Scribble should take care of the heavy lifting of setting up the documentation infrastructure, so you can get straight to writing stuff.

## Easy to use

Rather than a custom XML format or inventing a new DSL, Scribble will use Markdown for its content. This allows documentation to be:

 - header tags for sections
 - lists of things
 - code snippets
 - images
 - and of course words

## Easy to export

From your set of Markdown files you should be able to export a set of HTML/CSS/JS files so that you can publish the site elsewhere.

Jekyll is a common static site generator for Ruby, and I've worked on a [project](https://github.com/Code52/pretzel) in the past which allows you to create the site without installing Ruby on the machine. While I'm leaning towards that as a site structure, the blog-focus for some of the features may mean there's something more focused out there...

## Leverage your code

Quite often you will have code snippets which you want to include in your documentation. It might be in the form of a sample application or some unit tests, but being able to link code and documentation makes life so much easier. 

Scribble should allow you to annotate your source code, which you can then reference in documentation pages.
