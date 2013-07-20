---
title: Parsing LINQ-to-JSON
layout: default
---

# Parsing LINQ-to-JSON

LINQ to JSON has methods available for parsing JSON from a string or loading JSON directly from a file.
    
## Parsing JSON text
      
JSON values can be read from a string using  `<codeEntityReference>M:Newtonsoft.Json.Linq.JToken.Parse(System.String)</codeEntityReference>`.

### Parsing a JSON Object from text
<!-- import LinqToJsonCreateParse -->

### Parsing a JSON Array from text
<!-- import LinqToJsonCreateParseArray -->

## Loading JSON from a file

JSON can also be loaded directly from a file using `<codeEntityReference>M:Newtonsoft.Json.Linq.JToken.ReadFrom(Newtonsoft.Json.JsonReader)</codeEntityReference>`

### Reading JSON from a file
<!-- import LinqToJsonReadObject -->

#### See Also

 - [LINQ to JSON](linq-to-json.html)

**BF:** not really sure how to "migrate" this across, or whether i even want to. So I'll leave it here as a code snippet for starters

    <relatedTopics>
      <codeEntityReference>M:Newtonsoft.Json.Linq.JToken.Parse(System.String)</codeEntityReference>
      <codeEntityReference>M:Newtonsoft.Json.Linq.JToken.ReadFrom(Newtonsoft.Json.JsonReader)</codeEntityReference>
    </relatedTopics>