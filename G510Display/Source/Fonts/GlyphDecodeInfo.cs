using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G510Display.Source.Fonts
{
  class GlyphDecodeInfo
  {
    public GlyphDecodeInfo(BDF_Font_Header FontHeader, Int32 Pos)
    {
      GlyphPos = Pos;
      BitOffset = 0;
      OffsetX = 0;
      OffsetY = FontHeader.BoundingBoxHeight + FontHeader.BoundingBoxOffsetY;
      BitmapWidth = 0;
      BitmapHeight = 0;
      BitmapX = 0;
      BitmapY = 0;
    }
    public void SetGlyphInfo(BDF_Glyph_Header GlyphHeader)
    {
      OffsetX = GlyphHeader.BitmapOffsetX;
      OffsetY -= GlyphHeader.BitmapOffsetY + GlyphHeader.BitmapHeight;
      BitmapWidth = GlyphHeader.BitmapWidth;
      BitmapHeight = GlyphHeader.BitmapHeight;
    }
    public void MoveOnePixel()
    {
      BitmapX++;
      if (BitmapX >= BitmapWidth)
      {
        BitmapX = 0;
        BitmapY++;
      }
    }
    public bool IsBitmapReady()
    {
      return (BitmapY >= BitmapHeight);
    }
    public Int32 GlyphPos;
    public Int32 BitOffset;
    public Int32 OffsetX;
    public Int32 OffsetY;
    public Int32 BitmapWidth;
    public Int32 BitmapHeight;
    public Int32 BitmapX;
    public Int32 BitmapY;
  }
}
