using System;
using System.Collections.Generic;
using G510Display.Source.DrawImage;
using G510Display.Source.Logitech;

namespace G510Display.Source
{
  class Lcd
  {
    DImage TestImage;
    bool Loaded = false;
    bool IsKey0Pressed = false;
    bool IsKey1Pressed = false;
    bool IsKey2Pressed = false;
    bool IsKey3Pressed = false;
    LcdKeyCB KeyCb;

    public void Init(LcdKeyCB LcdKeyCb)
    {
      TestImage = new DImage();
      Loaded = LogitechInterface.LogiLcdInit("G510Display", LogitechInterface.LOGI_LCD_TYPE_MONO);
      TestImage.NewImage(LogitechInterface.LOGI_LCD_MONO_WIDTH, LogitechInterface.LOGI_LCD_MONO_HEIGHT, false, false, false);
      KeyCb = LcdKeyCb;
    }
    public void LcdWrite(int lineNumber, String text)
    {
      if (!Loaded) return;
      LogitechInterface.LogiLcdMonoSetText(lineNumber, text);
    }

    public void Update()
    {
      if (!Loaded) return;
      LogitechInterface.LogiLcdMonoSetBackground(TestImage.GetImage());
      LogitechInterface.LogiLcdUpdate();
    }
    public void Clear()
    {
      TestImage.DrawClear();
      TestImage.DrawHLine(0, 0, 160, 255);
      TestImage.DrawHLine(0, 1, 160, 255);
      TestImage.DrawHLine(0, 2, 160, 255);
      TestImage.DrawHLine(0, 3, 160, 255);
      TestImage.DrawHLine(0, 4, 160, 255);
      TestImage.DrawHLine(0, 5, 160, 255);
      TestImage.DrawHLine(0, 6, 160, 255);
    }

    public void PollKeys()
    {
      if (IsKey0Pressed != LogitechInterface.LogiLcdIsButtonPressed(LogitechInterface.LOGI_LCD_MONO_BUTTON_0))
      {
        IsKey0Pressed = !IsKey0Pressed;
        if (IsKey0Pressed)
          KeyCb.OnKey0Pressed();
        else
          KeyCb.OnKey0Released();
      }

      if (IsKey1Pressed != LogitechInterface.LogiLcdIsButtonPressed(LogitechInterface.LOGI_LCD_MONO_BUTTON_1))
      {
        IsKey1Pressed = !IsKey1Pressed;
        if (IsKey1Pressed)
          KeyCb.OnKey1Pressed();
        else
          KeyCb.OnKey1Released();
      }

      if (IsKey2Pressed != LogitechInterface.LogiLcdIsButtonPressed(LogitechInterface.LOGI_LCD_MONO_BUTTON_2))
      {
        IsKey2Pressed = !IsKey2Pressed;
        if (IsKey2Pressed)
          KeyCb.OnKey2Pressed();
        else
          KeyCb.OnKey2Released();
      }

      if (IsKey3Pressed != LogitechInterface.LogiLcdIsButtonPressed(LogitechInterface.LOGI_LCD_MONO_BUTTON_3))
      {
        IsKey3Pressed = !IsKey3Pressed;
        if (IsKey3Pressed)
          KeyCb.OnKey3Pressed();
        else
          KeyCb.OnKey3Released();
      }
    }

    public void LcdWriteTime()
    {
      TestImage.Image.ModeInverse = true;
      TestImage.Image.ModeTransparent = false;
      TestImage.Font_4x6_tf.DrawString(5, 1, DateTime.Now.ToLongDateString());
      TestImage.Font_4x6_tf.DrawStringRightAligned(157, 1, DateTime.Now.ToLongTimeString());
      TestImage.Image.ModeInverse = false;
      TestImage.Image.ModeTransparent = true;
    }

    public void LcdWrite(int ItemNr, CalendarItem Item)
    {
      TimeSpan Duration = Item.Start - DateTime.Now;
      int StartMinute = (int) (Duration.TotalHours * 60.0) + 1;

      string s;

      if (StartMinute < 10)
        s = string.Format("{0,5}  {1}", StartMinute, Item.Subject);
      else if (StartMinute < 100)
        s = string.Format("{0,4}  {1}", StartMinute, Item.Subject);
      else
        s = string.Format("{0,3}  {1}", StartMinute, Item.Subject);
      
      TestImage.Font_4x6_tf.DrawStringRightAligned(17, (ItemNr + 1) * TestImage.Font_4x6_tf.GetYSpacing() + 1, StartMinute.ToString());
      TestImage.Font_4x6_tf.DrawString(20, (ItemNr + 1) * TestImage.Font_4x6_tf.GetYSpacing() + 1, Item.Subject);
    }
    public void LcdWrite(EmailItem Item)
    {
      String InfoString1 = "From: " + Item.From;
      String InfoString2 = "Subject: " + Item.Subject;

      G510Display.Source.Fonts.Font DrawFont = TestImage.Font_4x6_tf;

      DrawFont.DrawString(0, LogitechInterface.LOGI_LCD_MONO_HEIGHT - (2 * DrawFont.GetYSpacing()), InfoString1);
      DrawFont.DrawString(0, LogitechInterface.LOGI_LCD_MONO_HEIGHT - (1 * DrawFont.GetYSpacing()), InfoString2);
    }
  }
  public class LcdKeyCB
  {
    virtual public void OnKey0Pressed() { }
    virtual public void OnKey0Released() { }
    virtual public void OnKey1Pressed() { }
    virtual public void OnKey1Released() { }
    virtual public void OnKey2Pressed() { }
    virtual public void OnKey2Released() { }
    virtual public void OnKey3Pressed() { }
    virtual public void OnKey3Released() { }
  }
}
