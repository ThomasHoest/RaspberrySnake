namespace Snake
{
  public class Apple : IEntity
  {
    public Direction Direction { get; set; }
    public Pixel Position { get; set; }
    
    public Apple(int x, int y)
    {
      Position = new Pixel(x,y);
      Position.R = 200;
      Position.G = 0;
      Position.B = 0;
    }

    public void Move()
    {
    }

    public void Draw(IGraphics g)
    {
      g.DrawPixel(Position);
    }
  }
}