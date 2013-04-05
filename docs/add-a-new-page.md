# [Proposal] Add a new page

Once you've created your docs folder, you can start creating new pages for your documentation.

`Add-Page "This is a page name"`

This should add a new file, named `this-is-a-page-name.md`, inside the `docs` folder.

It should open the new file inside Visual Studio, which is pre-populated from a predefined layout and content.

If you want to edit a page, don't bother with going to Explorer, just start typing...

`Edit-Page this-is-a-page-name`

We should use Powershell's autocomplete hooks and serve up a list of files which exist on disk.