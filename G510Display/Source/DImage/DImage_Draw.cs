using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G510Display.Source.DrawImage
{
  partial class DImage
  {
    public void DrawHLine(int x, int y, int Length, UInt32 Color)
    {
      Image.CursorX = x;
      Image.CursorY = y;
      for (int i = 0; i < Length; i++)
      {
        Image.PutPixel(i, 0, true);
      }
    }
    public void DrawVLine(int x, int y, int Length, UInt32 Color)
    {
      Image.CursorX = x;
      Image.CursorY = y;
      for (int i = 0; i < Length; i++)
      {
        Image.PutPixel(0, i, true);
      }
    }
  }
}
