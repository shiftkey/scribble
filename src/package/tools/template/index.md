---
title: Welcome to Scribble!
layout: default
---

# Welcome to Scribble

## Introduction

Congratulations, you've successfully installed Scribble - a documentation tool for .NET projects leveraging NuGet, Visual Studio, and a collection of open source projects. 

This is a tool to make your life easier by doing the heavy lifting of creating and maintaining beautiful documentation.

Specifically it is designed to:

 - make documentation easy and fun to write
 - keep code and documentation in sync
 - take your documentation and go further

### What is this index.md file? 

This is a template which has been added to the `docs` folder by Scribble after installing the package. 

Scribble will let you preview this at any time by pointing your browser at [localhost:40000](http://localhost:40000/).

Feel free to edit this file to your heart's content (and check it into source control) so that you too can start writing documentation for this project.

### What is this file used for?

This file uses Markdown syntax to annotate text files with a simple annotations which are then transformed into HTML. 

[this is a hyperlink to the current page](/index.html)

*This is some italics text*

**This is some bold text**

 - This is an unordered list
 - This is an unordered list
 - This is an unordered list

 1. This is an ordered list
 1. This is an ordered list
 1. This is an ordered list

`This is a code snippet`

    // This is a multi-line code snippet
    // which uses four spaces to indicate where the code starts
    public class Foo {
        public string Bar { get; set; }
    }

Markdown has become very popular on sites such as StackOverflow and GitHub because the syntax is good enough to cover what developers need to express questions and explain code, so it's a good pick for a documentation tool.

### And how is this MarkDown content being used?

This document is the starting point of a pipeline which Scribble uses to:

 - detect when you make changes to a file
 - take the contents of your `docs` folder and pass it through the MarkDown parser
 - dump the site content contents to a temporary folder in AppData
 - host the site using a small webserver so that you can preview it immediately

Test it out for yourself. Make a small change to the file, save it, and reload the browser.

**TODO: wouldn't it be cool if I could get the browser to reload onfocus (or when Visual Studio loses focus).**

### Woah! where's the styles coming from?

When you installed Scribble, we also included a simple site template and folder structure that is based on the `Jekyll` project.

**TODO: borrow and adapt a good template**

**TODO: can i hook pre- and post-processing to pretzel?**

If you're familiar with how Jekyll works, you can dive right in and start writing custom templates and layouts. We use a tool called `pretzel` to render the Jekyll site, and it supports a lot of the features that Jekyll does. 

If you're not, don't worry. Scribble adds some conventions to how Jekyll works so that you don't need to worry about it to gain the benefits.

### OK, so now what?

Just do a Find-Replace for Your-Project-Here and replace it with the name of your project.

Once you're familiar with this, delete this section and everything above it. The next section explains more about the features available to you and how to proceed.

## Your-Project-Here Documentation

### About

You should use this section here to explain what your project does and how it would benefit users. 

Here's a bunch of links to resources for this project:

 - [Project Site](https://github.com/shiftkey/scribble/)
 - [Documentation](https://shiftkey.github.com/scribble/)
 - [Issue Tracker](https://github.com/shiftkey/scribble/issues/)
 - [JabbR chatroom](https://jabbr.com/rooms/scribble)

### Installation

Getting started with Your-Project-Here is super-easy. Just install it from NuGet.

    Install-Package Your-Package-Here

You are using NuGet to distribute this project, right? You should. It's awesome.

### Samples

Right, so that's all fairly straight-forward documentation. Now we get into the fun stuff.

So, let's say you have some code snippets, like this:

    [Fact]
    public void This_Test_Uses_A_Cool_Api()
    {
        var foo = FooFactory.GetMeAFoo();
        var bar = foo.ThisIsAwesome();
        var baz = bar.ErmagherdStuff();
        Assert.Equal(1, baz);
    } 

But I don't want to paste this code into multiple places in my documentation. That's dumb. So instead I can use some specific tags to annotate my source code.

    [Fact]
    public void This_Test_Uses_A_Cool_Api()
    {
        // start code cool-api 
        var foo = FooFactory.GetMeAFoo();
        var bar = foo.ThisIsAwesome();
        var baz = bar.ErmagherdStuff();
        // end code cool-api
        Assert.Equal(1, baz);
    } 

This is inspired from some of the conventions which ReSharper uses to allow you to mute warnings in code. But in our case, we're using it to mark sections of code for use elsewhere.

The convention is this:

    // in a file inside your repository
    // start code {key}
    // ...
    // code to be used as value for key {key}
    // ...
    // end code {key}

If you were doing something with XML-like syntax (HTML/XAML/XML/etc), we could support this syntax:

    <!-- start code {key} -->
    <strong>this is something cool</strong>
    <em>but i really don't like XML</em>
    <!-- end code {key} -->

No spaces in the key - I'll freak out (and the parser will too)
Single-quotes or Double-quotes might work - but I want to keep this simple.

If there is a mismatch between start and end keys, or a key could not be found, you should get a build warning message. Nested keys should work, if you want to really drive me insane.

After doing that, this is where the real goddamn cheating is, you write yourself a HTML comment in your Markdown file, like this: `<!-- code {key} -->`

Parsers which don't understand the tag will just pass it through as a comment tag, and Scribble needs to keep the placeholders around to keep code in sync.

On saving the file, Scribble will:

 - parse the MarkDown file and find the comment tag
 - attempt to find the value for `{key}` 
 - use the value in your file as a code snippet

So when you build the project, Scribble will insert the code (as a raw snippet) after the comment tag. If the file is unsaved (has modified changes), Scribble will wait until the file has changed before it performs the processing.

If you want to leverage something like GitHub's syntax highlighting, you can add in an optional language after the key, like this: `<!-- code {key} csharp -->`.