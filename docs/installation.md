Duchess should be available as a solution-level NuGet package:

  - it should behave like a NuGet tool (installed into tools)
  - it should add some Powershell commands into the Package Manager Console

One package to rule them all, but perhaps some custom packages for additional extensions (way in the future basically).

### Why?

 - "projects" (this is now an overloaded term, perhaps I need to define it) are generally done per-repository and once a repository gets big enough it is broken out into independent repositories within an organisation over time (see ReactiveUI, Glimpse, SignalR).
 - code samples for documentation might be spread over different projects - having to install the package into multiple packages feels like a bad smell.
 - the code and documentation should be loosely coupled - we might need some magic to keep them in sync, but that's a separate responsibility right now...

### Implemented

When you install the first version of Duchess, you'll see it create the docs folder and open the index.md file. Yay progress!

### What's next?

 - More robust checks (don't clobber existing files etc)
 - better installer readme explaining what's going on
 - open browser to specific page inside VS? external browser?

