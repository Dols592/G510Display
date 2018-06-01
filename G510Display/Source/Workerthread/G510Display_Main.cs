using System;
using System.Collections.Generic;
using G510Display.Source.DataManager;

namespace G510Display.Source.Workerthread
{
  class G510Display_Main : LcdKeyCB
  {
    private DateTime NextReadCalendar = DateTime.MinValue;
    private DateTime NextUpdateLcd = DateTime.MinValue;
    private DateTime NextUpdatePollLcdKeys = DateTime.MinValue;
    Lcd Lcd = new Lcd();
    CDataManager DataManager = new CDataManager();

    public void Init()
    {
      Lcd.Init(this);
    }
    public void DoMainCycle()
    {
      //Lcd.Image.DrawLine(0, 0, 160, 40, 255);
      //Lcd.Image.DrawLine(20, 0, 140, 40, 255);
      //Lcd.Image.DrawLine(40, 0, 120, 40, 255);
      //Lcd.Image.DrawLine(60, 0, 100, 40, 255);
      //Lcd.Image.DrawLine(80, 0, 80, 40, 255);
      //Lcd.Image.DrawLine(100, 0, 60, 40, 255);
      //Lcd.Image.DrawLine(120, 0, 40, 40, 255);
      //Lcd.Image.DrawLine(140, 0, 20, 40, 255);
      //Lcd.Image.DrawLine(160, 0, 0, 40, 255);

      //Lcd.Image.DrawLine(0, 10, 160, 30, 255);
      //Lcd.Image.DrawLine(0, 20, 160, 20, 255);
      //Lcd.Image.DrawLine(0, 30, 160, 10, 255);

      //Lcd.Image.DrawCircle(20, 20, 20, 255);
      //Lcd.Image.DrawCircleFilled(80, 20, 20, 255);

      Lcd.Image.DrawBox(10, 10, 40, 30, 255);
      Lcd.Image.DrawBoxFilled(80, 10, 140, 30, 255);


      Lcd.Update();

      //CheckIfActionIsNeeded();
    }
    private void CheckIfActionIsNeeded()
    {
      DateTime TimestampNow = DateTime.Now;
      if (TimestampNow > NextReadCalendar)
        DoReadCalendarsAndEmails();

      if (TimestampNow > NextUpdateLcd)
        DoUpdateLcd();

      if (TimestampNow > NextUpdatePollLcdKeys)
        DoPollLcdKeys();
    }
    private void DoReadCalendarsAndEmails()
    {
      DataManager.ReadCalendars();
      DataManager.ReadEmails();
      DoUpdateLcd();
      NextReadCalendar = DateTime.Now.AddMinutes(1);
    }
    private void DoUpdateLcd()
    {
      Lcd.Clear();
      if (!Lcd.IsKey3Pressed)
      {
        Lcd.LcdWriteTime();

        List<CalendarItem> CalenderItems = DataManager.GetCalenderItems();

        Int32 LineCount = 0;
        for (int i = 0; i < 4; i++)
        {
          if (CalenderItems.Count > i)
            Lcd.LcdWrite(LineCount++, CalenderItems[i]);
        }

        List<EmailItem> EmailItems = DataManager.GetEmailItems();

        if (EmailItems.Count > 0)
        {
          Lcd.LcdWrite(EmailItems[0]);
        }
        Lcd.Update();
      }
      NextUpdateLcd = DateTime.Now.AddMilliseconds(100);
    }
    private void DoPollLcdKeys()
    {
      Lcd.PollKeys();
      NextUpdatePollLcdKeys = DateTime.Now.AddMilliseconds(50);
    }
    override public void OnKey3Pressed()
    {
      Lcd.Clear();
      Lcd.Image.Font9x15.DrawString(15, 7, "Refreshing", true, false);
      Lcd.Image.Font9x15.DrawString(80, 20, "Data...", true, false);
      Lcd.Update();
    }
    override public void OnKey3Released()
    {
      DoReadCalendarsAndEmails();
    }
  }
}
