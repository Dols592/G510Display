﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace G510Display.Source.Workerthread
{
  class ThreadPolling : LcdKeyCB
  {
    private bool stopping;
    private ManualResetEvent stoppedEvent;
    private DateTime NextReadCalendar = DateTime.MinValue;
    private DateTime NextUpdateLcd = DateTime.MinValue;
    private DateTime NextUpdatePollLcdKeys = DateTime.MinValue;
    List<CalendarItem> CalendarItems;
    List<EmailItem> EmailItems;
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
      CalendarItems = ReadExchange.ReadTodaysCalendarItems();
      EmailItems = ReadExchange.CheckForNewMail();
      DoUpdateLcd();
      NextReadCalendar = DateTime.Now.AddMinutes(1);
    }
    private void DoUpdateLcd()
    {
      Lcd.Clear();
      Lcd.LcdWriteTime();

      if (CalendarItems.Count <= 0)
      {
        Lcd.LcdWrite(1, "No Items.");
      }
      else
      {
        for (int i = 0; i < 6; i++)
        {
          if (CalendarItems.Count > i)
            Lcd.LcdWrite(i, CalendarItems[i]);
        }

      }
      if (EmailItems.Count > 0)
      {
        Lcd.LcdWrite(EmailItems[0]);
      }
      Lcd.Update();
      NextUpdateLcd = DateTime.Now.AddMilliseconds(100);
    }
    private void DoPollLcdKeys()
    {
      Lcd.PollKeys();
      NextUpdatePollLcdKeys = DateTime.Now.AddMilliseconds(50);
    }
  }
}
