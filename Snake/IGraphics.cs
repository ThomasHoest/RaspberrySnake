using System.Collections.Generic;

namespace Snake
{
  public interface IGraphics
  {
    void DrawPixels(IEnumerable<Pixel> pixels);
    void DrawPixel(Pixel position);
  }
}