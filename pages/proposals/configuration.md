---
layout: page
---   

## [Proposal] Configuration

It'd be great if you could set configurations to your scribble.json file which will you could then check into source control and NEVER HAVE TO WORRY ABOUT THEM AGAIN.

Something like this:

    Configure-Scribble "key" "value"

How would you discover what values are present?

    Configure-Scribble
    Configure-Scribble -List

I've broken something. How do I reset it?

    Configure-Scribble -Reset