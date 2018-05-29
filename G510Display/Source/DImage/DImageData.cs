using System;
using G510Display.Source.Fonts;

namespace G510Display.Source.DrawImage
{
  class DImageData : PutPixelInterface
  {
    public bool IsColor { get; set; }
    public bool IsColorBGR { get; set; }
    public bool IsAlphaLow { get; set; }
    public byte[] ImageData { get; set; }

    public UInt32 Width { get; set; }
    public UInt32 Height { get; set; }
    public UInt32 DataSize { get; set; }
    public bool IsCursorRightAligned { get; set; }

    public DImageData()
    {
      IsColor = false;
      IsColorBGR = false;
      IsAlphaLow = false;
      Width = 0;
      Height = 0;
      DataSize = 0;
      IsCursorRightAligned = false;
    }
    public override void PutPixel(Int32 x, Int32 y, bool Foreground)
    {
      if (ModeTransparent && !Foreground) return;

      UInt32 ColorValue = 0;
      if (Foreground) ColorValue = ~ColorValue;
      if (ModeInverse) ColorValue = ~ColorValue;

      PutPixelGray(x + CursorX, y + CursorY, (byte) ColorValue);
    }
    public void PutPixelRGBA(Int32 x, Int32 y, byte R, byte G, byte B, byte A)
    {
      if (x < 0) return;
      if (y < 0) return;
      if (x >= Width) return;
      if (y >= Height) return;

      UInt32 PixelPos = (((UInt32) y) * Width) + ((UInt32) x);
      if (IsColor)
      {
        PixelPos *= 4;
        ImageData[PixelPos] = A;
        ImageData[PixelPos+1] = R;
        ImageData[PixelPos+2] = G;
        ImageData[PixelPos+3] = B;
      }
    }
    public void PutPixelGray(Int32 x, Int32 y, byte Color)
    {
      if (x < 0) return;
      if (y < 0) return;
      if (x >= Width) return;
      if (y >= Height) return;

      UInt32 PixelPos = (((UInt32) y) * Width) + ((UInt32) x);
      if (IsColor)
      {
        PixelPos *= 4;
        ImageData[PixelPos] = 0;
        ImageData[PixelPos+1] = Color;
        ImageData[PixelPos+2] = Color;
        ImageData[PixelPos+3] = Color;
      }
      else
      {
        ImageData[PixelPos] = Color;
      }
    }

  }
}
