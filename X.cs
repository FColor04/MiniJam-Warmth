using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MiniJam_Warmth;


namespace SystemDebugTools {

    public class X {


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