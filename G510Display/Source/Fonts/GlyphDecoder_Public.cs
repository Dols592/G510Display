using System;
using System.Collections.Generic;

namespace G510Display.Source.Fonts
{
  public partial class Font
  {
    public Font(string OctalString, CFontPutPixelCB PixelWriter)
    {
      FontHeader = LoadFont(OctalString);
      PutPixelWriter = PixelWriter;
    }
    public void DrawString(Int32 X, Int32 Y, String Text, bool Transparant = true, bool Inversed = false)
    {
      CursorX = X;
      CursorY = Y;
      ModeInverse = Inversed;
      ModeTransparent = Transparant;

      for (int i = 0; i < Text.Length; i++)
      {
        Int32 GlyphPos = GetCharPos(Text[i]);
        DrawGlyph(GlyphPos, false);
      }        
    }
    public void DrawStringRightAligned(Int32 X, Int32 Y, String Text, bool Transparant = true, bool Inversed = false)
    {
      CursorX = X;
      CursorY = Y;
      ModeInverse = Inversed;
      ModeTransparent = Transparant;

      for (int i = Text.Length - 1; i >= 0; i--)
      {
        Int32 GlyphPos = GetCharPos(Text[i]);
        DrawGlyph(GlyphPos, true);
      }
    }
    public Int32 GetYSpacing()
    {
      return FontHeader.BoundingBoxHeight;
    }
    public Int32 GetPixelLength(String Text)
    {
      Int32 PixelWidth = 0;
      for (int i = 0; i < Text.Length; i++)
      {
        Int32 GlyphPos = GetCharPos(Text[i]);
        GlyphDecodeInfo DecodeInfo = new GlyphDecodeInfo(FontHeader, GlyphPos);
        BDF_Glyph_Header GlyphHeader = ReadGlyphHeader(ref DecodeInfo);
        PixelWidth += GlyphHeader.BitmapPitch;
      }

      return PixelWidth;
    }
  }
}
