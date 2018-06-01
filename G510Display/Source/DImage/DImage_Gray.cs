using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G510Display.Source.DImage
{
  public class DImage_Gray : DImage_Abstract
  {
    public override uint GetBytesPerLine() {return Width;}
    public override UInt32 GetBytesPerPixel() {return 1;}
    public override void PutPixel(Int32 x, Int32 y, bool Foreground)
    {
      PutPixel(x, y, (Byte) (Foreground ? 255 : 0));
    }
    public override void PutPixel(Int32 x, Int32 y, byte R, byte G, byte B, byte A)
    {
      UInt32 ColorGray = (2126u * R) + (7152u * G) + (722u * B); //Integer coverion for luminance
      ColorGray *= 255u; //Needed for Alpha convertion
      ColorGray /= 100000u * A; //To extract Luninance back to 0-255 (*255 for alpha channel still applied)
      ColorGray /= A; //apply Alpha channel

      PutPixel(x, y, (byte)ColorGray);
    }
    public override void PutPixel(Int32 x, Int32 y, UInt32 Color)
    {
      UInt32 ColorGray = Color & 0xFF; //just take the lower 8 bits
      PutPixel(x, y, (byte)ColorGray);
    }
    public override void PutPixel(Int32 x, Int32 y, byte Color)
    {
      if (x < 0) return;
      if (y < 0) return;
      if (x >= Width) return;
      if (y >= Height) return;

      Int32 PixelPos = y * (Int32) Width + x;
      ImageData[PixelPos] = Color;
    }
  }
}
