using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;

namespace Snake
{
  public class SnakeGame
  {
    private GraphicsWrapper _graphics;
    private Timer _gameTime;
    public TimeSpan _gameTick;
    private Snake _snake;
    private Apple _apple;
    private bool _paused;
    private bool _moved;
    private int _applesMunched;

    public List<IEntity> EntityList { get; set; }


    public SnakeGame()
    {
      InitGame();
      _graphics = new GraphicsWrapper();
      _gameTick = TimeSpan.FromMilliseconds(300);
      _gameTime = new System.Timers.Timer(_gameTick.TotalMilliseconds);
      _gameTime.Elapsed += (s, e) => RunGameTick();
      //_graphics.Test();
    }

    private void InitGame()
    {
      EntityList = new List<IEntity>();
      _snake = new Snake();
      EntityList.Add(_snake);
      AddApple();
    }

    public void Start()
    {
      _gameTime.Enabled = true;
      _paused = false;
    }

    public void Pause()
    {
      _gameTime.Enabled = false;
      _paused = true;
    }

    public void Stop()
    {
      _gameTime.Enabled = false;
      InitGame();
    }

    private void RunGameTick()
    {
      Console.WriteLine("Game tick");
      _graphics.Clear();
      foreach (var entity in EntityList)
      {
        entity.Move();
        entity.Draw(_graphics);
      }

      if (_apple.Position.X == _snake.Position.X && _apple.Position.Y == _snake.Position.Y)
      {
        EntityList.Remove(_apple);
        _snake.Grow();
        AddApple();
        _applesMunched++;

        if (_applesMunched % 4 == 0)
        {
          _gameTime.Interval -= 20;
        }
      }


      if (_snake.HeadBodyCollision())
      {
        Stop();
        DieEffect();
      }

      _graphics.Show();
      _moved = false;
    }

    private void DieEffect()
    {
      for (int i = 0; i<5; i++)
      {
        _graphics.Fill(200, 0, 0);
        _graphics.Show();
        Thread.Sleep(200);
        _graphics.Fill(0, 0, 0);
        _graphics.Show();
        Thread.Sleep(100);
      }

      _graphics.Fill(200, 0, 0);
      _graphics.Show();
    }

    public void AddApple()
    {
      Random r = new Random();
      int x = r.Next()%8;
      int y = r.Next() % 8;
      Apple a = new Apple(x,y);
      EntityList.Add(a);
      _apple = a;
    }

    public void Execute(ConsoleKey key)
    {
      if (_paused || _moved)
      {
        return;
      }

      switch (key)
      {
        case ConsoleKey.LeftArrow:
          if (_snake.Direction != Direction.Right)
          {
            _snake.Direction = Direction.Left;
          }
          break;
        case ConsoleKey.UpArrow:
          if (_snake.Direction != Direction.Down)
          {
            _snake.Direction = Direction.Up;
          }
          break;
        case ConsoleKey.RightArrow:
          if (_snake.Direction != Direction.Left)
          {
            _snake.Direction = Direction.Right;
          }
          break;
        case ConsoleKey.DownArrow:
          if (_snake.Direction != Direction.Up)
          {
            _snake.Direction = Direction.Down;
          }
          break;
      }

      _moved = true;
    }
  }
}
