# [Proposal] Import Code Snippet 

Now we get into the fun stuff.

So, let's say you have a code snippet, like this:

    [Fact]
    public void This_Test_Uses_A_Cool_Api()
    {
        var foo = FooFactory.GetMeAFoo();
        var bar = foo.ThisIsAwesome();
        var baz = bar.ErmagherdStuff();
        Assert.Equal(1, baz);
    } 

But I don't want to paste this code into multiple places in my documentation. That's dumb. 

So instead I can use some specific tags to annotate my source code.

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

This is inspired from conventions which ReSharper uses to allow you to mute warnings in code. But in our case, we're using it to flag sections of code for use elsewhere.

The convention is this:

    // in a file inside your repository
    // start code {key} {language}
    // ...
    // code to be used as value for key {key}
    // ...
    // end code {key}

If you were doing something with XML-like syntax (HTML/XAML/XML/etc), we could support this syntax:

    <!-- start code {key} -->
    <strong>this is something cool</strong>
    <em>but i really don't like XML</em>
    <!-- end code {key} -->

The syntax rules are:

 - No spaces in the key - use dashes if that's your thing
 - If there is a mismatch between start and end keys, or a key could not be found, you should get a build warning message. 
 - Nested keys could work, if you want to really drive me insane.

After doing that, this is where the real goddamn cheating is, you write yourself a HTML comment in your Markdown file, like this: `<!-- code {key} -->`

Parsers which don't understand the tag will just pass it through as a comment tag, and Scribble needs to keep the placeholders around to keep code in sync.

On saving the file, Scribble will:

 - parse the MarkDown file and find the comment tag
 - attempt to find the value for `{key}` 
 - use the value in your file as a code snippet

So when you build the project, Scribble will insert the code (as a raw snippet) after the comment tag. If the file is unsaved (has modified changes), Scribble will wait until the file has changed before it performs the processing.

If you want to leverage something like GitHub's syntax highlighting, you can add in an optional language after the key, like this: `<!-- code {key} csharp -->`.