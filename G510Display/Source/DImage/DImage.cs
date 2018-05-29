using System;
using System.Linq;
using G510Display.Source.Fonts;

namespace G510Display.Source.DrawImage
{
  partial class DImage
  {
    public DImageData Image;
    public Font Font5x7;
    public Font Font7x13;
    public Font Font9x15;
    public Font Font_4x6_tf;

    public DImage()
    {
      Image = new DImageData();
      Font5x7 = new Font(G510Display.Source.Fonts.FontData.Font5x7, Image);
      Font5x7 = new Font(G510Display.Source.Fonts.FontData.Font5x7, Image);
      Font7x13 = new Font(G510Display.Source.Fonts.FontData.Font7x13, Image);
      Font9x15 = new Font(G510Display.Source.Fonts.FontData.Font9x15, Image);
      Font_4x6_tf = new Font(G510Display.Source.Fonts.FontData.Font_4x6_tf, Image);
    }
    public void NewImage(UInt32 Width, UInt32 Height, bool IsColor, bool IsColorBGR, bool IsAlphaLow)
    {
      Image.IsColor = IsColor;
      Image.IsColorBGR = IsColorBGR;
      Image.IsAlphaLow = IsAlphaLow;
      Image.Width = Width;
      Image.Height = Height;
      Image.DataSize = Width * Height;
      if (IsColor) Image.DataSize *= 4;
      Image.ImageData = Enumerable.Repeat((byte) 0, (int)Image.DataSize).ToArray();

      //fonts.DrawString("Richard Dols", 2, 2, fonts.Font5x7, ref Image);
    }
    public byte[] GetImage()
    {
      return Image.ImageData;
    }
    public void DrawClear()
    {
      if (Image.ModeInverse)
        Image.ImageData = Enumerable.Repeat((byte)255, (int)Image.DataSize).ToArray();
      else
        Image.ImageData = Enumerable.Repeat((byte)0, (int)Image.DataSize).ToArray();

    }
  }
}
