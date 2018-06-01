using G510Display.Source.Google;
using System;
using System.Collections.Generic;

namespace G510Display.Source.DataManager
{
  public enum ItemSource
  {
    Google1 = 0,
    Google2,
    Google3,
    Google4,
    Google5,
    Exchange1,
    Exchange2,
    Exchange3,
    Exchange4,
    Exchange5,
    Count
  }
  class CalendarItem
  {
    public CalendarItem()
    {
      Start = DateTime.MinValue;
      End = DateTime.MaxValue;
      Subject = "";
      Source = ItemSource.Count;
      IsWholeDay = false;
    }

    public DateTime Start;
    public DateTime End;
    public string Subject;
    public ItemSource Source;
    public bool IsWholeDay;
  }
  class EmailItem
  {
    public EmailItem()
    {
      From = "";
      Subject = "";
      ReceivedTimestamp = DateTime.Now;
    }

    public String From;
    public String Subject;
    public DateTime ReceivedTimestamp;
  }
  class CDataManager
  {
    List<CalendarItem> CalendarItems = new List<CalendarItem>();
    List<EmailItem> EmailItems = new List<EmailItem>();

    public void ReadCalendars()
    {
      CalendarItems.Clear();
      AddCalendarItems(ReadExchange.ReadTodaysCalendarItems());
      AddCalendarItems(GoogleCalendar.ReadTodaysCalendarItems());
    }

    public List<CalendarItem> GetCalenderItems()
    {
      return CalendarItems;
    }

    public void ReadEmails()
    {
      EmailItems.Clear();
      AddEmailItems(ReadExchange.CheckForNewMail());
    }
    public List<EmailItem> GetEmailItems()
    {
      return EmailItems;
    }

    private void AddCalendarItems(List<CalendarItem> NewItems)
    {
      //sort on start date/time
      foreach (var NewItem in NewItems)
      {
        Int32 InsertPos = CalendarItems.Count;
        foreach (var CalendarItem in CalendarItems)
        {
          if (NewItem.Start < CalendarItem.Start)
          {
            InsertPos = CalendarItems.IndexOf(CalendarItem);
            break;
          }
        }
        CalendarItems.Insert(InsertPos, NewItem);
      }
    }

    private void AddEmailItems(List<EmailItem> NewItems)
    {
      //sort on start date/time
      foreach (var NewItem in NewItems)
      {
        Int32 InsertPos = EmailItems.Count;
        foreach (var EmailItem in EmailItems)
        {
          if (NewItem.ReceivedTimestamp < EmailItem.ReceivedTimestamp)
          {
            InsertPos = EmailItems.IndexOf(EmailItem);
            break;
          }
        }
        EmailItems.Insert(InsertPos, NewItem);
      }
    }
  }
}
