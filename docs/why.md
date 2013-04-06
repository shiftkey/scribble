# Why create Scribble?

This idea started out of a few discussions I had with people at MVP Summit in February, and I wrote an [initial brief](https://gist.github.com/shiftkey/5007179) about this in a rush on the last day.

Now that I've had some time to settle and start protyping concepts, I wanted to flesh out more about what problems I am looking to solve.

### People like to complain about documentation

I spent a bunch of time sitting in on open source discussions, and a common thread throughout was **"Why does documentation suck for .NET projects?"**.

Things like:

 - "why do many projects don't have docs at all?"
 - "docs are really hard to find!"
 - "docs doesn't cover more than the basics!"
 - "out-of-sync docs are painful to work with!"

And it's not like there's a lack of options out there - you can check out tools like [Sandcastle](http://shfb.codeplex.com/), [GhostDoc](http://submain.com/products/ghostdoc.aspx), [Doxygen](http://www.stack.nl/~dimitri/doxygen/) and [Live Documenter](http://theboxsoftware.com/products/live-documenter/) to get a feel for what's out there.

### Are we documenting the right things?

What's the first thing that comes to mind when you think of .NET documentation? XML documentation, right?

While xml-docs have their place, its a very specific type of documentation for a specific use case - that is, Intellisense inside Visual Studio. Things that have built on top of that are essentially codegen tools.

Why aren't we capturing things other sorts of information?

 - code snippets
 - tutorials
 - API walkthroughs
 - releases details

### Others are doing things differently

TODO: find some good examples of other projects doing cool things with documentation

## We can do better 

I was involved with a team recently where we tackled this problem. We had an existing unloved CMS containing documentation and a team of developers working on a big system. Over time the value of existing documentation was declining due to its isolation from the development process.

We wanted something that was easy to write and keep alongside code. Something that we could version control. Something we could deploy out to an environment from a build server. 

Due to the size of the project, we had to drop this approach at a certain point - but it was an excellent fit for a team of developers working on a codebase.

Scribble is an evolution of what I learned from that project.