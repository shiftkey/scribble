# [Proposal] API Documentation Part A - Create Mini-Site

This is an open-ended discussion I've been having with a few people, but I'm almost at the point where I want to start developing these features so I'm going to elaborate first on the path I intend to take

## Why?

Part of any good library or piece of software is presenting it's API surface in an easy-to-navigate way. In many .NET projects this comes down to two things:

 - XML documentation which is read by Visual Studio and presented using the Intellisense features inside VS
 - MSDN-style documentation generated from the available XML documentation

If you've never seen XML documentation before, this is an example:

```
/// <summary>
///  This class performs an important function.
/// </summary>
public class MyClass{}
```

Fortunately I don't have to worry about the first feature, as that's taken care of by the people creating the library (they annotate their code) or by Visual Studio (they parse the generated XML). However that second task is something I will need to resolve as people continue to ask for it.

## Why MSDN-style documentation sucks

A quick note on why I continute to stall on implementing this:

### The value provided by this (ignoring Intellisense) is debatable

Pop quiz: what does this property do?

```
/// <summary>
/// Gets or sets the value for BlahBlah
/// </summary>
public int BlahBlah { get; set; }
```

That's right, it's just a plain old-fashioned property. So why even document it?

 - because Visual Studio can be configured to *demand* these comments (as part of compilation) on all public classes, method, properties and fields...
 - because tools like FxCop can be configured to *demand* comments in a certain format ('properties must start with a "Gets or sets" summary')...
 - because consumers *demand* XML documentation whenever they download a library, no matter how trivial some features might be...

### Reinventing the wheel (this time around) is not fun

One of the inspirations for this project was to go in a radically-different approach to what MSDN has done with documentation. I wanted to start from a blank slate, not from an XML file, and see what results.

### There's probably some other things but I'm getting sidetracked



## Walkthrough the feature

### Getting Started

So let's say we had installed Scribble but wanted to create a mini-site which was a simple way to 
navigate the API?

From the NuGet Package Manager we run:

    Configure-APIDocs

Which creates the necessary files:

 - _api/layouts/home.html (the landing page for the API mini-site)
 - _api/layouts/layout.html (if you want to style this area in a different way to the rest of the site)

Now, the name `_api` is important as any folder starting with `_` will be skipped by the Jekyll processor.

But this introduces a different problem - what layout should we be using here? 

I'm going to focus on .NET for the moment, but I think we can get away with *just* two layouts:

  - namespace.html 
  - class.html

These files will be editable by the user, so they can organise the data in whatever way they want.

TODO: outline what the model and it's properties will look like, as this will impact template options
TODO: what if we want to include page-specific data? Where will that live?

### Generating the mini-site

So we have our templates ready and our XML documentation handy, so we run

    Update-APIDocs

From the Package Manager Console and our mini-site is created. It'll look something like this:

 - /api/index.html
 - /api/Namespace/index.html
 - /api/Namespace/Class/index.html
 - /api/Namespace/Class/index.html#Member

Using `/api/` here is just a convention. and inside the class file we have anchor tags to indicate the specific members available.

TODO: how would the member syntax look with things like generic parameters and method overloads?

