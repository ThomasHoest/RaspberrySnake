using System;
using System.Collections.Generic;
using Unicornhat;

namespace Snake
{
  public class GraphicsWrapper : IGraphics
  {
    private ws2812 _unicorn;

    public GraphicsWrapper()
    {
      _unicorn = new ws2812();
      _unicorn.Init();
      _unicorn.SetBrightness(0.4F);
    }

    public void Clear()
    {
      _unicorn.Clear();
    }


    public void DrawPixels(IEnumerable<Pixel> pixels)
    {
      foreach (var p in pixels)
      {
        _unicorn.SetPixel(p.X, p.Y, p.R, p.G, p.B);  
      }
    }

    public void DrawPixel(Pixel p)
    {
      _unicorn.SetPixel(p.X, p.Y, p.R, p.G, p.B);  
    }

    public void Fill(byte r, byte g, byte b)
    {
      for (int i = 0; i<8; i++)
      {
        _unicorn.SetPixelRow(i, 255, r, g, b);
      }
    }

    public void Test()
    {
      Console.WriteLine("Testing");
      _unicorn.Clear();
      
      _unicorn.SetPixel(1, 1, 100, 100, 100);
      _unicorn.Show();
    }


    public void Show()
    {
      _unicorn.Show();
    }
  }
}