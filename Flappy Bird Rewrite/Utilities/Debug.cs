using System;

namespace Flappy_Bird_Rewrite.Utilities
{
    public static class Debug
    {
        public static string DebugText = "";
        public static void Write(string text)
        {
            Console.WriteLine(text);
            DebugText += text + "\n";
        }
    }
}