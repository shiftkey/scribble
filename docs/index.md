---
layout: index
---

No-one likes writing documentation because it's not fun. 

This is the problem Scribble is looking to solve.

### I want to try it out!

Pre-release packages are available on NuGet. If you're feeling bold, go try them out!

<div class="nuget-badge">
  <p><code>PM&gt; Install-Package scribble -Pre</code></p>
</div>

and then follow the [Getting Started](/scribble/pages/getting-started.html) guide.

### Why should I care?

#### Don't want to use "insert terrible syntax here"?

Scribble uses [Markdown](http://daringfireball.net/projects/markdown/) at it's core. If you've never heard of Markdown, it's a lightweight markup syntax for turning text into HTML. 

It's been used in question-and-answer sites like StackOverflow, various blogging engines and sites, and other place too I'll bet. 

It's easy to learn and has a feature set which fits with what developers need to describe their software, so it's a great match for what Scribble needs.

#### Want to include real code snippets?

Yes! Using code to demonstrate your features is excellent, but you'll probably want to use real code.

It just requires a bit of annotating and some commands (at the moment), but this let's you start referencing real code in your documentation without worrying about future API changes being forgotten in your documentation.

#### Want to quickly and easily publish?

Most people associate documentation for .NET projects as:

 - XML documentation
 - CHM files created from XML documentation

Scribble is not looking to go down this path, but is instead looking to borrow ideas from static site generators to make the outputs more interesting and valuable.