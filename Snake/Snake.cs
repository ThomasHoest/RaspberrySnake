using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
  public class Snake : IEntity
  {
    public Direction Direction { get; set; } // 0=l, 1=u, 2=d, 3=right
    public Pixel Position { get; set; }

    public void Grow()
    {
      Pixel p = Body[Body.Count - 1].Clone();
      Body.Add(p);
    }

    public Snake()
    {
      Body = new List<Pixel>(){
        new Pixel(4,4),
        new Pixel(4,5),
        new Pixel(4,6),
        new Pixel(4,7)
        };
      Direction = Direction.Up;
      Position = Body[0];
    }

    public void Move()
    {
      
      Pixel p = Position.Clone();
      switch (Direction)
      {
        case Direction.Up:
          p.X--;  
          break;
        case Direction.Down:
          p.X++;
          break;
        case Direction.Left:
          p.Y++;
          break;
        case Direction.Right:
          p.Y--;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      CheckBoundaries(p);
      Body.Insert(0,p);
      Position = p;
      Body.RemoveAt(Body.Count - 1);
      Console.WriteLine("Position  X " + Position.X + " Y " + Position.Y);
    }

    private void CheckBoundaries(Pixel p)
    {
      if (p.X > 7)
      {
        p.X = 0;
      }
      else if(p.X == -1)
      {
        p.X = 7;
      }
      else if (p.Y == -1)
      {
        p.Y = 7;
      }
      else if (p.Y > 7)
      {
        p.Y = 0;
      }
    }

    public void Draw(IGraphics g)
    {
      g.DrawPixels(Body);
    }

    public IList<Pixel> Body { get; set; }

    public bool HeadBodyCollision()
    {

      return Body.Where((p)=>!p.Equals(Position)).Any((p) => p.X == Position.X && p.Y == Position.Y);
    }
  }
}
