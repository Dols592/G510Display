using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using G510Display.Source.Google;

namespace G510Display.Source.Workerthread
{
  class ThreadPolling : LcdKeyCB
  {
    private bool stopping;
    private ManualResetEvent stoppedEvent;
    private DateTime NextReadCalendar = DateTime.MinValue;
    private DateTime NextUpdateLcd = DateTime.MinValue;
    private DateTime NextUpdatePollLcdKeys = DateTime.MinValue;
    List<CalendarItem> CalendarItemsExchange;
    List<EmailItem> EmailItemsExchange;
    List<CalendarItem> CalendarItemsGoogle;
    List<EmailItem> EmailItemsGoogle;
    Lcd Lcd = new Lcd();

    public void StartThread()
    {
      stopping = false;
      stoppedEvent = new ManualResetEvent(false);
      ThreadPool.QueueUserWorkItem(new WaitCallback(ServiceWorkerThread));
    }

    public void StopAndWait()
    {
      stopping = true;
      stoppedEvent.WaitOne();
    }
    private void ServiceWorkerThread(object state)
    {
      Lcd.Init(this);

      G510Display.Source.Google.GoogleCalendar.ReadTodaysCalendarItems();

      while (!stopping)
      {
        CheckIfActionIsNeeded();
        Thread.Sleep(10);
      }

      this.stoppedEvent.Set();
      if (!stopping)
      {
        TrayNotification.CurrentTrayNotification.CleanExit();
      }
    }
    private void CheckIfActionIsNeeded()
    {
      DateTime TimestampNow = DateTime.Now;
      if (TimestampNow > NextReadCalendar)
        DoReadExchange();

      if (TimestampNow > NextUpdateLcd)
        DoUpdateLcd();
	
      if (TimestampNow > NextUpdatePollLcdKeys)
        DoPollLcdKeys();
    }
    private void DoReadExchange()
    {
      CalendarItemsExchange = ReadExchange.ReadTodaysCalendarItems();
      EmailItemsExchange = ReadExchange.CheckForNewMail();
      CalendarItemsGoogle = GoogleCalendar.ReadTodaysCalendarItems();

      DoUpdateLcd();
      NextReadCalendar = DateTime.Now.AddMinutes(1);
    }
    private void DoUpdateLcd()
    {
      Lcd.Clear();
      Lcd.LcdWriteTime();

      Int32 LineCount = 0;
      for (int i = 0; i < 2; i++)
      {
        if (CalendarItemsExchange.Count > i)
          Lcd.LcdWrite(LineCount++, CalendarItemsExchange[i]);
      }
      for (int i = 0; i < 2; i++)
      {
        if (CalendarItemsGoogle.Count > i)
          Lcd.LcdWrite(LineCount++, CalendarItemsGoogle[i]);
      }

      if (EmailItemsExchange.Count > 0)
      {
        Lcd.LcdWrite(EmailItemsExchange[0]);
      }
      Lcd.Update();
      NextUpdateLcd = DateTime.Now.AddMilliseconds(100);
    }

    private void DoPollLcdKeys()
    {
      Lcd.PollKeys();
      NextUpdatePollLcdKeys = DateTime.Now.AddMilliseconds(50);
    }
    override public void OnKey3Pressed()
    {
      DoReadExchange();
    }
  }
}
