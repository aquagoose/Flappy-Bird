using System;

namespace Flappy_Bird_Rewrite
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new FlappyBird())
                game.Run();
        }
    }
}