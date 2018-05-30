using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Exchange.WebServices.Data;

namespace G510Display.Source
{
  struct CalendarItem
  {
    public DateTime Start;
    public DateTime End;
    public string Subject;
  }

  class ReadExchange
  {
    public static List<CalendarItem> ReadTodaysCalendarItems()
    {
      ItemView view = new ItemView(10);
      DateTime Today = DateTime.Today;
      DateTime Now = DateTime.Now;
      CalendarView calView = new CalendarView(Today, Today.AddDays(2));

      List<CalendarItem> CalendarItems = new List<CalendarItem>();

      FindItemsResults<Item> SearchResults = CreateExchangeConnection().FindItems(WellKnownFolderName.Calendar, calView);

      foreach (Item item in SearchResults.Items)
      {
        Appointment ItemAppointment = item as Appointment;
        if (Now < ItemAppointment.End)
        {
          CalendarItem NewCalendarItem;
          NewCalendarItem.Start = ItemAppointment.Start;
          NewCalendarItem.End = ItemAppointment.End;
          NewCalendarItem.Subject = ItemAppointment.Subject;
          CalendarItems.Add(NewCalendarItem);
        }
      }

      return CalendarItems;
    }
    private static ExchangeService CreateExchangeConnection()
    {
      ExchangeService EllipsService = new ExchangeService();
      EllipsService.Url = new Uri("https://autodiscover.ellips.com/EWS/Exchange.asmx");
      EllipsService.UseDefaultCredentials = true;
      //EllipsService.Credentials = new System.Net.NetworkCredential("User", "Password", "Domain");
      return EllipsService;
    }
  }
}
