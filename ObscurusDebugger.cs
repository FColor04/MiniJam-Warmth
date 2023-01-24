using Microsoft.VisualBasic;
using System.Diagnostics;


namespace ObscurusDebuggerTools {

    public class ObscurusDebugger {


        public bool DebugMode = false;
        public readonly int DebugInt = 5;
        public readonly float DebugFloat = 3.141592653589793238462643383279f;

        #region ~Log~
        public static void Log(string message)
        {
            Debug.WriteLine(message);
        }

        public static void Log(float Number)
        {
            Debug.WriteLine("Log: " + Number.ToString()); 
        }

        public static void LogWarning(string message)
        {
            Debug.WriteLine("[WARNING] " + message);
        }

        public static void LogError(string message)
        {
            Debug.WriteLine("[ERROR] " + message);
        }

        public static void ThrowError()
        {
            Debug.WriteLine("An Error Has Occurred...");
        }
        #endregion

        #region ~Tools~

        #endregion

    }
}