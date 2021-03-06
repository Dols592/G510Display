﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using G510Display.Source.DataManager;
using Microsoft.Exchange.WebServices.Data;

namespace G510Display.Source
{
  class ReadExchange
  {
    public static List<CalendarItem> ReadTodaysCalendarItems()
    {
      ItemView view = new ItemView(10);
      DateTime Start = DateTime.Now;
      DateTime End = DateTime.Today.AddDays(2);      
      CalendarView calView = new CalendarView(Start, End);

      List<CalendarItem> CalendarItems = new List<CalendarItem>();

      FindItemsResults<Item> SearchResults = CreateExchangeConnection().FindItems(WellKnownFolderName.Calendar, calView);

      foreach (Item item in SearchResults.Items)
      {
        Appointment ItemAppointment = item as Appointment;

        CalendarItem NewCalendarItem = new CalendarItem();
        NewCalendarItem.Start = ItemAppointment.Start;
        NewCalendarItem.End = ItemAppointment.End;
        NewCalendarItem.Subject = ItemAppointment.Subject;
        NewCalendarItem.Source = G510Display.Source.DataManager.ItemSource.Exchange1;
        CalendarItems.Add(NewCalendarItem);
      }

      return CalendarItems;
    }
    public static List<EmailItem> CheckForNewMail()
    {
      ItemView View = new ItemView(10);
      SearchFilter Filter = new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false);

      List<EmailItem> EmailItems = new List<EmailItem>();

      FindItemsResults<Item> SearchResults = CreateExchangeConnection().FindItems(WellKnownFolderName.Inbox, Filter, View);
      foreach (Item item in SearchResults.Items)
      {
        EmailMessage ItemEmail = item as EmailMessage;
        EmailItem NewEmailItem = new EmailItem();
        NewEmailItem.From= ItemEmail.Sender.Name;
        NewEmailItem.Subject = ItemEmail.Subject;
        NewEmailItem.ReceivedTimestamp = ItemEmail.DateTimeReceived;
        EmailItems.Add(NewEmailItem);
      }

      return EmailItems;
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
