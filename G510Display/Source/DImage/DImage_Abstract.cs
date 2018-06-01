using System;
using System.Linq;
using G510Display.Source.Fonts;

namespace G510Display.Source.DImage
{
  abstract public partial class DImage_Abstract : CFontPutPixelCB
  {
    public UInt32 Width { get; set; }
    public UInt32 Height { get; set; }

    abstract public UInt32 GetBytesPerLine(); //bytes per line
    abstract public UInt32 GetBytesPerPixel();
    protected Byte[] ImageData;

    public Font Font5x7;
    public Font Font7x13;
    public Font Font9x15;
    public Font Font_4x6_tf;

    public DImage_Abstract()
    {
      Width = 0;
      Height = 0;
      ImageData = new Byte[10];
    
    Font5x7 = new Font(G510Display.Source.Fonts.FontData.Font5x7, this);
    Font7x13 = new Font(G510Display.Source.Fonts.FontData.Font7x13, this);
    Font9x15 = new Font(G510Display.Source.Fonts.FontData.Font9x15, this);
    Font_4x6_tf = new Font(G510Display.Source.Fonts.FontData.Font_4x6_tf, this);
  }
  virtual public void NewImage(UInt32 NewWidth, UInt32 NewHeight)
    {
      Width = NewWidth;
      Height = NewHeight;

      UInt32 AllocationNeeded = Height * GetBytesPerLine();
      if (ImageData.Length < AllocationNeeded)
        ImageData = Enumerable.Repeat((byte)0, (int)AllocationNeeded).ToArray();
    }
    virtual public void Clear(Byte Value = 0)
    {
      ImageData = Enumerable.Repeat((byte)Value, (int)ImageData.Length).ToArray();
    }
    virtual public Byte[] GetData()
    {
      return ImageData;
    }
    override public void FontPutPixelCB(Int32 x, Int32 y, bool Foreground)
    {
      PutPixel(x, y, Foreground);
    }

    //putpixels
    abstract public void PutPixel(Int32 x, Int32 y, bool Foreground);
    abstract public void PutPixel(Int32 x, Int32 y, byte R, byte G, byte B, byte A);
    abstract public void PutPixel(Int32 x, Int32 y, UInt32 Color);
    abstract public void PutPixel(Int32 x, Int32 y, Byte Color);
  }
}
