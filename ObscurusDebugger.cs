using System;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;


namespace ObscurusDebuggerTools {

    public static class ObscurusDebugger {
        public static bool DebugMode;
        public static readonly int DebugInt = 5;
        public static readonly float DebugFloat = 3.141592653589793238462643383279f;

        private static string TimeNow => DateTime.Now.ToString("T");
        
        static ObscurusDebugger()
        {
#if DEBUG
            DebugMode = true;
#else
            DebugMode = Environment.GetCommandLineArgs().Any(arg => arg == "--debug");
#endif
        }
        
        #region ~Log~
        public static void Log(object obj)
        {
            if (!DebugMode) return;
            Console.WriteLine($"[{TimeNow}] {obj}"); 
        }

        public static void LogWarning(object obj)
        {
            if (!DebugMode) return;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[{TimeNow}] [WARNING] {obj}");
            Console.WriteLine($"{string.Join("\n", (new StackTrace(true)).GetFrames().Select(frame => $"{frame.GetMethod()} \n {frame.GetFileName()} @ {frame.GetFileLineNumber()}"))} line");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void LogError(object obj)
        {
            if (!DebugMode) return;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{TimeNow}] [ERROR] {obj}");
            Console.WriteLine($"{string.Join("\n", (new StackTrace(true)).GetFrames().Select(frame => $"{frame.GetMethod()} \n {frame.GetFileName()} @ {frame.GetFileLineNumber()}"))} line");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void ThrowError()
        {
            if (!DebugMode) return;
            LogError($"An error has occured");
        }
        #endregion

        #region ~Tools~

        public static void DebugDrawPoint(Vector2 screenCoordinates, Color color)
        {
            
        }
        #endregion

    }
}