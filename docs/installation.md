# Design Decisions

Duchess should be available as a solution-level NuGet package.

In particular:

  - it should behave like a NuGet tool (installed into tools)
  - it should add some Powershell commands into the Package Manager Console

I am not currently planning extensibility points, but will keep this philosophy in mind:

 - the core package should be sufficient for common scenarios
 - others could design custom NuGet packags which depend on the core Scribble
 - popular extensions could be integrated into the core based on demand

## Why?

 - "projects" (this is now an overloaded term, perhaps I need to define it) are generally done per-repository and once a repository gets big enough it is broken out into independent repositories within an organisation over time (see ReactiveUI, Glimpse, SignalR).
 - code samples for documentation might be spread over different projects - having to install the package into multiple packages feels like a bad smell.
 - the code and documentation should be loosely coupled - we might need some magic to keep them in sync, but that's a separate responsibility right now...

