using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G510Display.Source.Fonts
{
  partial class Font
  {
    PutPixelInterface PutPixelWriter;
    private Int32 GetCharPos(char Character)
    {
      int DataEnd = FontHeader.FontData.Length;
      int DataCur = 0;
      if (Character >= 0xFF)
        return -1;
      else if (Character >= 'a')
        DataCur = FontHeader.Pos_a;
      else if (Character >= 'A')
        DataCur = FontHeader.Pos_A;
      while (DataCur + 1 < DataEnd)
      {
        if (FontHeader.FontData[DataCur] == Character)
          return DataCur;

        DataCur += FontHeader.FontData[DataCur + 1];
      }
      return -1;
    }
    private void DrawGlyph(Int32 GlyphPos, bool RightAlignment)
    {
      GlyphDecodeInfo DecodeInfo = new GlyphDecodeInfo(FontHeader, GlyphPos);
      BDF_Glyph_Header GlyphHeader = ReadGlyphHeader(ref DecodeInfo);
      DecodeInfo.SetGlyphInfo(GlyphHeader);

      if (RightAlignment)
        PutPixelWriter.CursorX -= GlyphHeader.BitmapPitch;

      DrawGlyph(DecodeInfo, GlyphHeader);

      if (!PutPixelWriter.ModeTransparent)
        EraseRemainingBackground(GlyphHeader);

      if (!RightAlignment)
        PutPixelWriter.CursorX += GlyphHeader.BitmapPitch;

    }
    private void DrawGlyph(GlyphDecodeInfo DecodeInfo, BDF_Glyph_Header GlyphHeader)
    {
      while (!DecodeInfo.IsBitmapReady())
      {
        Int32 Pixels0 = DecodeBitFieldUnsigned(DecodeInfo, (byte)FontHeader.m0);
        Int32 Pixels1 = DecodeBitFieldUnsigned(DecodeInfo, (byte)FontHeader.m1);
        do
        {
          WritePixels(ref DecodeInfo, Pixels0, false);
          WritePixels(ref DecodeInfo, Pixels1, true);
        } while (DecodeBitFieldUnsigned(DecodeInfo, 1) == 1);
      }      
    }
    private void WritePixels(ref GlyphDecodeInfo DecodeInfo, Int32 NrOfPixels, bool Color)
    {
      for (int i = 0; i < NrOfPixels; i++)
      {
        if (DecodeInfo.IsBitmapReady())
          return;
        PutPixelWriter.PutPixel(DecodeInfo.BitmapX + DecodeInfo.OffsetX, DecodeInfo.BitmapY + DecodeInfo.OffsetY, Color);
        DecodeInfo.MoveOnePixel();
      }
    }
    private void EraseRemainingBackground(BDF_Glyph_Header GlyphHeader)
    {
      Int32 StartBitmapY = FontHeader.BoundingBoxHeight + FontHeader.BoundingBoxOffsetY - GlyphHeader.BitmapOffsetY - GlyphHeader.BitmapHeight;
      Int32 EndBitmapY = StartBitmapY + GlyphHeader.BitmapHeight;

      for (int Y = 0; Y < FontHeader.BoundingBoxHeight; Y++)
      {
        for (int X = 0; X < FontHeader.BoundingBoxWidth; X++)
        {
          if (Y >= StartBitmapY && Y < EndBitmapY)
          {
            if (X == GlyphHeader.BitmapOffsetX)
              X += GlyphHeader.BitmapWidth;
          }
          PutPixelWriter.PutPixel(X, Y, false);
        }
      }
    }
    private BDF_Glyph_Header ReadGlyphHeader(ref GlyphDecodeInfo DecodeInfo)
    {
      BDF_Glyph_Header GlyphHeader = new BDF_Glyph_Header();
      GlyphHeader.BitmapWidth = DecodeBitFieldUnsigned(DecodeInfo, FontHeader.GlyphWidthSizeInBits);
      GlyphHeader.BitmapHeight = DecodeBitFieldUnsigned(DecodeInfo, FontHeader.GlyphHeightSizeInBits);
      GlyphHeader.BitmapOffsetX = DecodeBitFieldSigned(DecodeInfo, FontHeader.GlyphOffetXSizeInBits);
      GlyphHeader.BitmapOffsetY = DecodeBitFieldSigned(DecodeInfo, FontHeader.GlyphOffetYSizeInBits);
      GlyphHeader.BitmapPitch = DecodeBitFieldSigned(DecodeInfo, FontHeader.GlyphPitchSizeInBits);
      return GlyphHeader;
    }
    private Int32 DecodeBitFieldUnsigned(GlyphDecodeInfo DecodeInfo, byte NrOfBits)
    {
      Int32 BytePos = DecodeInfo.GlyphPos + 2 + (DecodeInfo.BitOffset / 8);
      Int32 AlreadyReadBitsOfByte = DecodeInfo.BitOffset % 8;
      Int32 BitMask = (Int32)~(0xFFFFFFFF << NrOfBits);

      Int32 BytesValue = (Int32)(FontHeader.FontData[BytePos + 1] * 256) + FontHeader.FontData[BytePos];
      BytesValue = BytesValue >> AlreadyReadBitsOfByte;
      BytesValue = BytesValue & BitMask;

      DecodeInfo.BitOffset += NrOfBits;

      return BytesValue;
    }

    private Int32 DecodeBitFieldSigned(GlyphDecodeInfo DecodeInfo, byte NrOfBits)
    {
      Int32 v = DecodeBitFieldUnsigned(DecodeInfo, NrOfBits);
      Int32 d = 1 << (NrOfBits - 1);
      v -= d;

      return v;
    }
  }
}
