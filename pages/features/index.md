---
layout: page
---   

## Features

Here's a quick overview of the important features of Scribble:

### Preview Documentation

With your documentation in source control as Markdown files, you can browse these files at any time.

To start the pretzel webserver (which handles tranforming files as well as serving content) just run `Start-Preview` from the Package Manager Console.

![preview the file]({{ site.baseurl }}images/introduction/view-site.png)

For more details read the [Preview Documentation]({{ site.baseurl }}pages/features/preview-documentation.html) section.

### Jekyll Compatibility

If you've ever used [Jekyll](http://jekyllrb.com/), the way files are organised inside the Scribble docs folder should be familiar to you.

If you're not familiar with it, [this]({{ site.baseurl }}pages/features/folder-structure.html) should be a good introduction to it.

### Add Code Snippets

One of the first features underway (that I wanted from Sandcastle) is supporting embedding code snippets into your documentation.

    [Fact]
    public void This_Test_Uses_A_Cool_Api()
    {
        // start code this-is-a-code-snippet 
        var foo = FooFactory.GetMeAFoo();
        var bar = foo.ThisIsAwesome();
        var baz = bar.ErmagherdStuff();
        // end code this-is-a-code-snippet
        Assert.Equal(1, baz);
    } 

This eliminates the need to manually edit code samples as the underlying code and APIs change.

For more details read the [Code Snippets]({{ site.baseurl }}pages/features/code-snippets.html) section.