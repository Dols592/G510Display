using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G510Display.Source.Fonts
{
  public partial class Font
  {
    private BDF_Font_Header FontHeader;

    private BDF_Font_Header LoadFont(String RawFontData)
    {
      List<byte> FontData = DecodeOctalString(RawFontData);
      BDF_Font_Header Header = GetHeaderFromData(FontData);
      return Header;
    }

    private static List<byte> DecodeOctalString(string OctalString)
    {
      byte CurrentAsciiValue = 0;
      bool InOctet = false;
      List<byte> DecodedData = new List<byte>();
      for (int i = 0; i < OctalString.Length; i++)
      {
        byte CurrentChar = (byte)OctalString[i];
        if (InOctet)
        {
          if (CurrentChar >= '0' && CurrentChar <= '7')
          {
            CurrentAsciiValue = (byte)(CurrentAsciiValue << 3);
            CurrentAsciiValue += (byte)(CurrentChar - '0');
            continue;
          }
          else
          {
            DecodedData.Add(CurrentAsciiValue);
            CurrentAsciiValue = 0;
            InOctet = false;
          }
        }

        if (CurrentChar == '\\')
          InOctet = true;
        else
        {
          DecodedData.Add(CurrentChar);
        }
      }

      if (InOctet)
        DecodedData.Add(CurrentAsciiValue);

      return DecodedData;
    }
    private static BDF_Font_Header GetHeaderFromData(List<byte> FontData)
    {
      BDF_Font_Header Header = new BDF_Font_Header();
      Header.NumberOfGlyphs = FontData[0];
      Header.BoundingBoxMode = FontData[1];
      Header.m0 = FontData[2];
      Header.m1 = FontData[3];
      Header.GlyphWidthSizeInBits = FontData[4];
      Header.GlyphHeightSizeInBits = FontData[5];
      Header.GlyphOffetXSizeInBits = FontData[6];
      Header.GlyphOffetYSizeInBits = FontData[7];
      Header.GlyphPitchSizeInBits = FontData[8];
      Header.BoundingBoxWidth = (sbyte) FontData[9];
      Header.BoundingBoxHeight = (sbyte) FontData[10];
      Header.BoundingBoxOffsetX = (sbyte) FontData[11];
      Header.BoundingBoxOffsetY = (sbyte) FontData[12];
      Header.Ascent_A = (sbyte) FontData[13];
      Header.Descent_g = (sbyte) FontData[14];
      Header.Ascent_Hook = (sbyte) FontData[15];
      Header.Descent_Hook = (sbyte) FontData[16];

      Header.Pos_A = (UInt16)(FontData[17] * 256 + FontData[18]);
      Header.Pos_a = (UInt16)(FontData[19] * 256 + FontData[20]);
      Header.Pos_0x100 = (UInt16)(FontData[21] * 256 + FontData[22]);
      FontData.RemoveRange(0, 23);
      Header.FontData = FontData.ToArray();
      return Header;
    }
  }
}
