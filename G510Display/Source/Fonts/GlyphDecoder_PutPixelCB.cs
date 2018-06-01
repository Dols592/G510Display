using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G510Display.Source.Fonts
{
  public abstract class CFontPutPixelCB
  {
    public abstract void FontPutPixelCB(Int32 x, Int32 y, bool Foreground);
  }
}
