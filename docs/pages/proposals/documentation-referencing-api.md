---
layout: page
---   

# [Proposal] API Documentation Part B - Referencing API Documentation inside Scribble

So if we have our mini-site working as per the spec in Part A, how can we link to generated pages inside code? This is a tricky problem for several reasons:

 - references should not be tightly coupled (but I want to know when things are not in sync)
 - references should not be hard-coded (as my local environment will be different to deploying to github-pages, for example)
 - TODO: other reasons

## Some inspiration from Sandcastle

So Sandcastle has this neat feature called CodeReferences which are specific XML tags to point to members (class, method, whatever) inside XML documentation. Here's an example:

```
The simplest way to check if JSON is valid is to load the JSON into a JObject or JArray and then
use the <codeEntityReference>M:Newtonsoft.Json.Schema.Extensions.IsValid(Newtonsoft.Json.Linq.JToken,Newtonsoft.Json.Schema.JsonSchema)</codeEntityReference> method with the JSON Schema.
```

But that's awful. Just horrible. 

I'd rather do something like this:

```
The simplest way to check if JSON is valid is to load the JSON into a JObject or JArray and then
use the `Extensions.IsValid(jToken,jsonSchema)` method with the JSON Schema.
```

Notice the differences?

 - Using Markdown's code syntax means it'll be rendered as a `<code>` tag
 - It reads so much better in this way
 - I chose to specify the class and method rather than the whole type definition
 - Instead of specifying the full type definition of the parameters, I've used just the parameters names to indicate which implementation is actually being referred to.

TODO: what about non-.NET languages?
TODO: can this mapping syntax be extended?

Other examples:

 - `MyNamespace` - reference to the `/api/my-namespace/index.html` page
 - `MyClass` - reference to the `/api/my-namespace/my-class/index.html` page
 - `MyClass.Foo` - reference to the `/api/my-namespace/my-class/index.html#foo` page

TODO: what if we have two identical classes in an assembly? How can we explicitly state which reference is to be used?

### The other half of the problem

So we have a nice way of including references to code, but no way to wireup these links.

#### Tranform

As part of the `Update-APIDocs` step, the XML documentation is transformed into a simple format of key-value pairs (where the key is a piece of text and the value is a URL). This is the lookup values for any API references to use for hyperlinks, and likes at `/api/references.json`.

#### Load Resource

 - a small piece of JavaScript is written and linked in the footer of all the API documentation pages
 - when the user navigates to a page, in the background it fetches the `references.json` file  

 TODO: is there a reliable, cross-platform way to cache this data for a period of time? AppCache?

#### Enhance Document

Once the reference the JavaScript file will then search the current page for any `<code>` tags.

For each of these tags, if it's text matches a key in it's collection, the script will enhance the code tag by transforming it into a link - retaining the style but now becoming a clickable link to the relevant API documentation.

### Linking Code in other formats

While that might sound like a lot, it's barely the tip of the iceberg with what we might be able to support with assocating content together.

