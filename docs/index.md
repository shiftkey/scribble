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

#### If you don't want to use [insert terrible syntax]

The more you have to fight with the tool or framework, the easier it is to neglect documentation - which leads to atrophy and the existing documentation losing it's value.

Plus XML is almost certainly terrible for humans.

Scribble uses Markdown at it's core. If you've never heard of Markdown, it's a lightweight markup syntax for turning text into HTML. It's been used in question-and-answer sites like StackOverflow, various blogging engines and sites, and other place too I'll bet.

It's easy to learn and has a feature set which fits with what developers need to describe their software, so it's a great match for what Scribble needs.

#### If you want to include code snippets automagically

Yes! Using code to demonstrate your features is excellent, but you want to use your real code - not some copy-paste effort which means you'll have to edit it again later.

#### If you want to quickly and easily publish your documentation

Most people associate documentation for .NET projects as:

 - XML documentation
 - CHM files created from XML documentation

Scribble is not looking to go down this path, but is instead looking to borrow ideas from static site generators to make the outputs more interesting and valuable.

In fact, *the documentation for Scribble itself has been created using Scribble*.
