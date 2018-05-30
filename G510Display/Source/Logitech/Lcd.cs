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
      //Font = TestImage.fonts.Font_4x6_tf;
      Loaded = LogitechInterface.LogiLcdInit("G510Display", LogitechInterface.LOGI_LCD_TYPE_MONO);
      TestImage.NewImage(LogitechInterface.LOGI_LCD_MONO_WIDTH, LogitechInterface.LOGI_LCD_MONO_HEIGHT, false, false, false);
      //TestImage.DrawHLine(10, 10, 20, 0xFF);
      KeyCb = LcdKeyCb;
    }
    public void LcdWrite(int lineNumber, String text)
    {
      if (!Loaded) return;
      LogitechInterface.LogiLcdMonoSetText(lineNumber, text);
      //LogitechInterface.LogiLcdMonoSetText(1, "1234567890");
      //LogitechInterface.LogiLcdMonoSetText(2, "12345678901234567890");
      //LogitechInterface.LogiLcdMonoSetText(3, "123456789012345678901234567890");
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
    }

    public void LcdWriteTime()
    {
      String s = DateTime.Now.ToLongDateString() + "  " + DateTime.Now.ToLongTimeString();
      TestImage.Font_4x6_tf.DrawString(0, 0, s);
      //TestImage.DrawString(s, 0, 0, Font);
      //LogitechInterface.LogiLcdMonoSetText(0, s);
    }

    public void LcdWrite(int ItemNr, NextItem Item)
    {
      //27 characters on 1 line
      TimeSpan Duration = Item.Start - DateTime.Now;
      int StartMinute = (int) (Duration.TotalHours * 60.0) + 1;

      string s;

      if (StartMinute < 10)
        s = string.Format("{0,5}  {1}", StartMinute, Item.Subject);
      else if (StartMinute < 100)
        s = string.Format("{0,4}  {1}", StartMinute, Item.Subject);
      else
        s = string.Format("{0,3}  {1}", StartMinute, Item.Subject);

      TestImage.Font_4x6_tf.DrawStringRightAligned(17, (ItemNr + 1) * TestImage.Font_4x6_tf.GetYSpacing(), StartMinute.ToString());
      TestImage.Font_4x6_tf.DrawString(18, (ItemNr + 1) * TestImage.Font_4x6_tf.GetYSpacing(), Item.Subject);
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
