﻿using System;
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
      ItemView view = new ItemView(5);
      DateTime Today = DateTime.Today;
      DateTime Now = DateTime.Now;
      CalendarView calView = new CalendarView(Today, Today.AddDays(2));

      List<CalendarItem> NextItems = new List<CalendarItem>();

      FindItemsResults<Item> SearchResults = CreateExchangeConnection().FindItems(WellKnownFolderName.Calendar, calView);

      foreach (Item item in instanceResults.Items)
      {
        Appointment appointment = item as Appointment;
        if (Now < appointment.End)
        {
          CalendarItem NewItem;
          NewItem.Start = appointment.Start;
          NewItem.End = appointment.End;
          NewItem.Subject = appointment.Subject;
          NextItems.Add(NewItem);
        }
      }

      return NextItems;
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
