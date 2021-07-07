using System;

namespace smartSnek {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("<Start>");
            Game g = new Game(15,15);
            Console.ReadKey();
            while(1==1) {
                g.step();
                Console.Clear();
                Console.WriteLine(g.ToString());
                ConsoleKeyInfo inf = Console.ReadKey();
                switch(inf.Key) {
                    case ConsoleKey.LeftArrow:
                        g.Left();
                        break;
                    case ConsoleKey.RightArrow:
                        g.Right();
                        break;
                    case ConsoleKey.UpArrow:
                        g.Up();
                        break;
                    case ConsoleKey.DownArrow:
                        g.Down();
                        break;
                }
            }

        }
    }
}
