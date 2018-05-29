using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G510Display.Source.Fonts
{
  public struct BDF_Font_Header
  {
    public byte NumberOfGlyphs;
    public byte BoundingBoxMode;
    public byte m0;
    public byte m1;
    public byte GlyphWidthSizeInBits;
    public byte GlyphHeightSizeInBits;
    public byte GlyphOffetXSizeInBits;
    public byte GlyphOffetYSizeInBits;
    public byte GlyphPitchSizeInBits;
    public sbyte BoundingBoxWidth;
    public sbyte BoundingBoxHeight;
    public sbyte BoundingBoxOffsetX;
    public sbyte BoundingBoxOffsetY;
    public sbyte Ascent_A;
    public sbyte Descent_g;
    public sbyte Ascent_Hook;
    public sbyte Descent_Hook;
    public UInt16 Pos_A;
    public UInt16 Pos_a;
    public UInt16 Pos_0x100;
    public byte[] FontData;
  }
  public struct BDF_Glyph_Header
  {
    public Int32 BitmapWidth;
    public Int32 BitmapHeight;
    public Int32 BitmapOffsetX;
    public Int32 BitmapOffsetY;
    public Int32 BitmapPitch;
    public Int32 BitOffset;
  }
}
