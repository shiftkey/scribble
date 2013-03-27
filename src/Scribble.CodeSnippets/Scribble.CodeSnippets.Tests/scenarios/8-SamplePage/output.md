# This is a Title

This is some words about the code below. Look below!

<!-- import LinqToJsonBasic -->
    JObject o = JObject.Parse(@"{
    'CPU': 'Intel',
    'Drives': [
      'DVD read/writer',
      '500 gigabyte hard drive' ]
    }");
    
    string cpu = (string) o["CPU"];
    // Intel
    
    string firstDrive = (string) o["Drives"][0];
    // DVD read/writer
    
    IList<string> allDrives = o["Drives"].Select(t => (string) t).ToList();
    // DVD read/writer
    // 500 gigabyte hard drive

And we're now below the code snippet. Cool eh?