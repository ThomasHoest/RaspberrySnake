namespace Snake
{
  public interface IEntity
  {
    Direction Direction { get; set; }
    void Move();
    void Draw(IGraphics g);
    Pixel Position { get; set; }
  }
}