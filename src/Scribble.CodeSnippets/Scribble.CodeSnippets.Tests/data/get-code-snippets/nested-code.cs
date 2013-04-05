  public class LinqToJsonTests
  {
    public void LinqToJsonBasic()
    {
      // start code LinqToJsonBasic
      JObject o = JObject.Parse(@"{
        'CPU': 'Intel',
        'Drives': [
          'DVD read/writer',
          '500 gigabyte hard drive'
        ]
      }");

      // start code LinqToJsonBasic-Results
      string cpu = (string)o["CPU"];
      // Intel

      string firstDrive = (string)o["Drives"][0];
      // DVD read/writer

      IList<string> allDrives = o["Drives"].Select(t => (string)t).ToList();
      // DVD read/writer
      // 500 gigabyte hard drive
      // end code LinqToJsonBasic-Results
      // end code LinqToJsonBasic
    }
