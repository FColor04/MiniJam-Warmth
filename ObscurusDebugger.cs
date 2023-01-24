using System.Diagnostics;


namespace ObscurusDebuggerTools {

    public class ObscurusDebugger {


        public bool DebugMode;

        public static void Log(string message)
        {
            Debug.WriteLine(message);
        }

        public static void LogWarning(string message)
        {
            Debug.WriteLine("[WARNING] " + message);
        }

        public static void LogError(string message)
        {
            Debug.WriteLine("[ERROR] " + message);
        }
    }
}