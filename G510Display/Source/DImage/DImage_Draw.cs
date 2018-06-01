using System;

namespace G510Display.Source.DImage
{
  abstract public partial class DImage_Abstract
  {
    public void DrawHLine(int x, int y, int Length, UInt32 Color)
    {
      if (Length < 0)
      {
        Length = -Length;
        x = x - Length + 1;
      }
      for (int i = 0; i < Length; i++)
      {
        PutPixel(x + i, y, Color);
      }
    }
    public void DrawVLine(int x, int y, int Length, UInt32 Color)
    {
      if (Length < 0)
      {
        Length = -Length;
        y = y - Length + 1;
      }
      for (int i = 0; i < Length; i++)
      {
        PutPixel(x, y+i, Color);
      }
    }
    public void DrawLine(Int32 x1, Int32 y1, Int32 x2, Int32 y2, UInt32 Color)
    {
      if (x1 == x2)
        DrawVLine(x1, y1, y2 - y1, Color);
      else if (y1 == y2)
        DrawHLine(x1, y1, x2 - x1, Color);
      else if (Math.Abs(y2 - y1) < Math.Abs(x2 - x1))
      {
        if (x1 > x2)
          DrawLineOnX(x2, y2, x1, y1, Color);
        else
          DrawLineOnX(x1, y1, x2, y2, Color);
      }
      else
      {
        if (y1 > y2)
          DrawLineOnY(x2, y2, x1, y1, Color);
        else
          DrawLineOnY(x1, y1, x2, y2, Color);
      }
    }
    public void DrawCircle(Int32 xc, Int32 yc, Int32 r, UInt32 Color)
    {      
      PutPixel(xc - r, yc, Color);
      PutPixel(xc + r, yc, Color);
      PutPixel(xc, yc - r, Color);
      PutPixel(xc, yc + r, Color);
      Int32 r2 = r * r;

      for (Int32 x = 1; x < r; x++)
      {
        Int32 y = (Int32) (Math.Sqrt(r2 - (x * x)) + 0.5);
        PutPixel(xc + x, yc + y, Color);
        PutPixel(xc - x, yc + y, Color);
        PutPixel(xc + x, yc - y, Color);
        PutPixel(xc - x, yc - y, Color);
        PutPixel(xc + y, yc + x, Color);
        PutPixel(xc - y, yc + x, Color);
        PutPixel(xc + y, yc - x, Color);
        PutPixel(xc - y, yc - x, Color);
      }
    }
    public void DrawCircleFilled(Int32 xc, Int32 yc, Int32 r, UInt32 Color)
    {
      PutPixel(xc, yc - r, Color);
      PutPixel(xc, yc + r, Color);
      DrawHLine(xc - r, yc, 2* r + 1,  Color);
      Int32 r2 = r * r;

      for (Int32 x = 1; x < r; x++)
      {
        Int32 y = (Int32)(Math.Sqrt(r2 - (x * x)) + 0.5);
        DrawHLine(xc - x, yc + y, x * 2 + 1, Color);
        DrawHLine(xc - x, yc - y, x * 2 + 1, Color);        
        DrawHLine(xc - y, yc + x, y * 2 + 1, Color);
        DrawHLine(xc - y, yc - x, y * 2 + 1, Color);
      }
    }

    public void DrawBox(Int32 x1, Int32 y1, Int32 x2, Int32 y2, UInt32 Color)
    {
      DrawHLine(x1, y1, x2 - x1 + 1, Color);
      DrawHLine(x1, y2, x2 - x1 + 1, Color);
      DrawVLine(x1, y1+1, y2 - y1 - 1, Color);
      DrawVLine(x2, y1 + 1, y2 - y1 - 1, Color);
    }
    public void DrawBoxFilled(Int32 x1, Int32 y1, Int32 x2, Int32 y2, UInt32 Color)
    {
      if (y2 < y1)
      {
        Int32 y3 = y1;
        y1 = y2;
        y2 = y3;
      }
      for (Int32 y = y1; y <= y2; y++)
      {
        DrawHLine(x1, y, x2 - x1 + 1, Color);
      }
    }

    private void DrawLineOnX(Int32 x1, Int32 y1, Int32 x2, Int32 y2, UInt32 Color)
    {
      Int32 dx = x2 - x1;
      Int32 dy = y2 - y1;
      Int32 yi = 1;
      if (dy < 0)
      {
        yi = -1;
        dy = -dy;
      }
      Int32 D = 2 * dy - dx;
      Int32 y = y1;

      for (Int32 x = x1; x <= x2; x++)
      {
        PutPixel(x, y, Color);
        if (D > 0)
        {
          y += yi;
          D -= 2 * dx;
        }
        D += 2 * dy;
      }
    }
    private void DrawLineOnY(Int32 x1, Int32 y1, Int32 x2, Int32 y2, UInt32 Color)
    {
      Int32 dx = x2 - x1;
      Int32 dy = y2 - y1;
      Int32 xi = 1;
      if (dx < 0)
      {
        xi = -1;
        dx = -dx;
      }
      Int32 D = 2 * dx - dy;
      Int32 x = x1;

      for (Int32 y= y1; y <= y2; y++)
      {
        PutPixel(x, y, Color);
        if (D > 0)
        {
          x += xi;
          D -= 2 * dy;
        }
        D += 2 * dx;
      }
    }
  }
}
