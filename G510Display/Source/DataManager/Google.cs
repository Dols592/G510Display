using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using G510Display.Source.DataManager;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;


namespace G510Display.Source.Google
{
  class GoogleCalendar
  {
    static string[] Scopes = { CalendarService.Scope.CalendarReadonly };
    static string ApplicationName = "G510Display";
    static byte[] ApiArray = { 185, 179, 107, 222, 88, 164, 136, 194, 50, 166, 224, 153, 97, 152, 64, 202, 111, 146, 107, 146, 217, 89, 28, 177, 235, 214, 32, 250, 3, 216, 128, 99, 147, 161, 175, 179, 121, 83, 65, 95, 131, 169, 254, 126, 35, 162, 142, 241, 128, 64, 56, 204, 222, 232, 62, 232, 195, 176, 132, 123, 156, 153, 199, 206, 151, 173, 134, 18, 17, 28, 246, 248, 181, 207, 187, 212, 137, 239, 230, 229, 182, 185, 144, 7, 239, 15, 112, 146, 134, 58, 233, 96, 182, 163, 42, 189, 168, 107, 197, 117, 20, 188, 235, 77, 61, 97, 155, 157, 140, 121, 219, 142, 161, 233, 179, 34, 87, 159, 102, 17, 200, 121, 243, 238, 135, 21, 148, 184, 127, 14, 146, 132, 82, 96, 5, 224, 237, 141, 10, 96, 118, 64, 125, 68, 89, 194, 182, 231, 190, 165, 154, 81, 165, 224, 216, 43, 122, 30, 1, 219, 231, 48, 171, 208, 50, 223, 60, 244, 76, 246, 235, 180, 182, 72, 6, 22, 243, 118, 14, 162, 209, 111, 151, 97, 27, 243, 60, 92, 223, 47, 172, 121, 250, 2, 35, 189, 252, 184, 174, 136, 72, 0, 0, 115, 118, 160, 63, 178, 27, 206, 208, 242, 101, 119, 187, 223, 177, 70, 16, 246, 246, 243, 88, 43, 22, 240, 0, 118, 89, 250, 220, 138, 46, 45, 10, 164, 62, 27, 155, 245, 143, 12, 44, 34, 128, 109, 16, 192, 96, 32, 2, 0, 119, 114, 99, 132, 57, 190, 214, 197, 148, 253, 112, 184, 59, 133, 5, 202, 187, 50, 11, 80, 127, 54, 102, 197, 244, 214, 0, 231, 126, 169, 50, 230, 245, 254, 50, 214, 167, 120, 194, 77, 108, 183, 105, 163, 111, 212, 190, 72, 78, 118, 163, 250, 210, 23, 47, 244, 134, 115, 205, 189, 166, 108, 84, 72, 239, 19, 37, 17, 71, 58, 159, 15, 242, 57, 251, 97, 238, 96, 54, 42, 185, 26, 177, 82, 113, 230, 77, 169, 206, 237, 41, 251, 55, 158, 25, 47, 230, 176, 120, 57, 103, 97, 72, 177, 79, 221, 14, 250, 212, 181, 114, 134, 84, 247, 114, 98, 237, 239, 113, 36, 26, 160, 54, 103, 151, 59, 241, 183, 124, 251, 227, 119, 42, 244, 107, 63, 122, 227, 197, 55, 224, 20, 140, 96, 93, 139, 149, 33, 209, 224, 35, 25, 156, 172, 216, 29, 149, 107, 41, 112, 176, 8, 97, 207, 196, 138, 218, 98, 86, 118, 195, 223, 148, 76, 8, 167, 33, 76, 121, 208, 29, 168, 96, 148, 222, 234, 87, 31, 161, 220, 40, 196, 142, 253, 196, 222, 53, 99, 41, 243, 211, 104, 168, 168, 141, 122, 66, 220, 203, 66, 230, 32, 160, 97, 21, 91, 60, 65, 102, 62, 250, 10, 67, 111, 180, 71, 101, 14, 138, 159, 212, 115, 79, 140, 86, 164, 63, 41, 223, 243, 215, 155, 41, 172, 116, 140, 170, 143, 28, 8, 35, 224, 70, 128, 108, 72, 159, 13, 56, 2 };
    static byte[] ApiKey = { 49, 188, 168, 119, 96, 100, 91, 161, 53, 170, 113, 121, 86, 197, 44, 223, 250, 74, 229, 110, 145, 22, 40, 216, 55, 69, 171, 43, 44, 213, 153, 127, 205, 187, 121, 158, 155, 152, 47, 99, 86, 35, 131, 173, 81, 130, 224, 112, 66, 111, 128, 0, 247, 83, 58, 173, 207, 245, 76, 44, 73, 4, 109, 246};
    public static List<CalendarItem> ReadTodaysCalendarItems()
    {
#if false
      using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
      {
        long Size = stream.Length;
        byte[] Data = new byte[Size + 10];
        int ReadSize = stream.Read(Data, 0, (int)Size);
        string Json = System.Text.Encoding.Default.GetString(Data);
        byte[] Encrypted = G510Display.Source.StringEncrypter.StringCipher.Encrypt(Json, ApiKey);
      }
#endif

      UserCredential GoogleCredential = G510Display.Source.StringEncrypter.StringCipher.AuthorizeGoogle(ApiArray, ApiKey);

      // Create Google Calendar API service.
      var service = new CalendarService(new BaseClientService.Initializer()
      {
        HttpClientInitializer = GoogleCredential,
        ApplicationName = ApplicationName,
      });

      // Define parameters of request.
      EventsResource.ListRequest request = service.Events.List("primary");
      request.TimeMin = DateTime.Now;
      request.TimeMax = DateTime.Today.AddDays(2);
      request.ShowDeleted = false;
      request.SingleEvents = true;
      request.MaxResults = 10;
      request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

      // List events.
      List<CalendarItem> CalendarItems = new List<CalendarItem>();
      Events events = request.Execute();
      if (events.Items != null && events.Items.Count > 0)
      {
        foreach (var eventItem in events.Items)
        {
          CalendarItem NewCalendarItem = new CalendarItem();
          if (eventItem.Start.DateTime != null && eventItem.End.DateTime != null)
          {
            NewCalendarItem.Start = (DateTime)eventItem.Start.DateTime;
            NewCalendarItem.End = (DateTime)eventItem.End.DateTime;
            NewCalendarItem.IsWholeDay = false;
          }
          else if (eventItem.Start.Date != null && eventItem.End.Date != null)
          {
            NewCalendarItem.Start = DateTime.Parse(eventItem.Start.Date);
            NewCalendarItem.End = DateTime.Parse(eventItem.End.Date);
            NewCalendarItem.IsWholeDay = true;
          }
          else
            continue;

          NewCalendarItem.Subject = eventItem.Summary;
          NewCalendarItem.Source = G510Display.Source.DataManager.ItemSource.Google1;
          CalendarItems.Add(NewCalendarItem);
        }
      }
      return CalendarItems;
    }
  }
}
