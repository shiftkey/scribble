# [Proposal] Visual Studio Integration (VSIX)

Why?

 - you have to open the Package Manager Console to access package functionality - which sucks
 - EnvDTE gives you a lot of access to the project information and APIs, but the IDE integration is shallow

Ideas

 - hook into VS build events so that you can update code snippets that actually compile!
 - easy access to documentation (think folder view in Sublime Text) without needing to access Package Manager Console
 
