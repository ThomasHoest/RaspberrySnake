namespace Snake
{
  public class Pixel
  {
    public Pixel(int x, int y)
    {
      X = x;
      Y = y;
      R = 92;
      G = 230;
      B = 0;
    }

    public int X { get; set; }
    public int Y { get; set; }
    public byte R { get; set; }
    public byte G { get; set; }
    public byte B { get; set; }

    public Pixel Clone()
    {
      return new Pixel(X,Y);
    }
    
  }
}