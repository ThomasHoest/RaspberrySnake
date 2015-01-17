using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
  class Program
  {
    static void Main(string[] args)
    {

      try
      {
        SnakeGame game = new SnakeGame();
        ConsoleKeyInfo cki;
        do
        {
          cki = Console.ReadKey();

          if (cki.Key == ConsoleKey.S)
          {
            Console.WriteLine("Starting game");
            game.Start();
          }
          else
          {
            Console.WriteLine("Key pressed");
            game.Execute(cki.Key);
          }

        } while (cki.Key != ConsoleKey.Escape);

        Console.WriteLine("Game stopped");
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        Console.WriteLine("CRASH!!!!");
      }
    }
  }
}
