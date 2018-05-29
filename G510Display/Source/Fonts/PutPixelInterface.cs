using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G510Display.Source.Fonts
{
  public abstract class PutPixelInterface
  {
    public PutPixelInterface()
    {
      CursorX = 0;
      CursorY = 0;
      ModeTransparent = true;
      ModeInverse = false;
    }
    public abstract void PutPixel(Int32 x, Int32 y, bool Color);
    public Int32 CursorX;
    public Int32 CursorY;
    public bool ModeTransparent;//do not write background
    public bool ModeInverse;
  }
}
