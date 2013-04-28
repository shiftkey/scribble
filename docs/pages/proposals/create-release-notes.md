---
layout: page
---   

## [Proposal] Create Release Notes

### Workflow

To make it easy to document as you develop, why not support a workflow that makes it easy to capture release notes?

### Get Started

From the Package Manager Console, just tell Scribble what you're up to.

If you want to start a new release:

```
scribble start major
```

If you're doing something smaller, these are all equivalent:

    scribble start bugfix
    scribble start minor
    scribble start feature
    scribble start task

If you want to do a pre-release version, any of these things will work:

    scribble start preview
    scribble start pre

### What happens next

So rather than worrying about "ensuring you're doing semver right", why not leave it up to the underlying framework?

Under the hood, scribble will:

 - create a folder called `docs\release-notes\` (if it doesn't already exist) at the root
 - generate a Markdown template for your release notes (using semanticreleasenotes.org conventions)
 - the file will be named to match the version (e.g. 1.0.md)
 - add in some infrastructure to take this version and update your assemblies in build

TODO:

 - need an opinionated and nice way to hook into pre-build events and change the AssemblyInfo version
 - need to do opinionated integration with .gitignore/.hgignore files
 - needs to work outside of VS (guess this means build targets uuuugh)

### Versioning

The version it chooses will be based on some conventions:

 - `preview` or `pre` - start at 0.1
 - `major` - start at 1.0

And then you can create interim releases:

 - `pre`, then `minor` - 0.1 -> 0.2
 - and then `minor` again - 0.2 -> 0.3

Once you get to a stable, you can then flag the thing is ready for use:

 - `pre`, then `minor`, then `major` - 0.1 -> 0.2 -> 1.0
 - and then a minor release - 1.0 -> 1.1
 