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
    string json = @"{
      CPU: 'Intel',
      Drives: [
        'DVD read/writer',
        '500 gigabyte hard drive'
      ]
    }";
    
    JObject o = JObject.Parse(json);

### Parsing a JSON Array from text
<!-- import LinqToJsonCreateParseArray -->
    string json = @"[
      'Small',
      'Medium',
      'Large'
    ]";
    
    JArray a = JArray.Parse(json);

## Loading JSON from a file

JSON can also be loaded directly from a file using `<codeEntityReference>M:Newtonsoft.Json.Linq.JToken.ReadFrom(Newtonsoft.Json.JsonReader)</codeEntityReference>`

### Reading JSON from a file
<!-- import LinqToJsonReadObject -->
    using (StreamReader reader = File.OpenText(@"c:\person.json"))
    {
      JObject o = (JObject)JToken.ReadFrom(new JsonTextReader(reader));
      // do stuff
    }

#### See Also

 - [LINQ to JSON](linq-to-json.html)

**BF:** not really sure how to "migrate" this across, or whether i even want to. So I'll leave it here as a code snippet for starters

    <relatedTopics>
      <codeEntityReference>M:Newtonsoft.Json.Linq.JToken.Parse(System.String)</codeEntityReference>
      <codeEntityReference>M:Newtonsoft.Json.Linq.JToken.ReadFrom(Newtonsoft.Json.JsonReader)</codeEntityReference>
    </relatedTopics>