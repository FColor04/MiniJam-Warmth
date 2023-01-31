using System;
using System.Runtime.InteropServices;
using MainGameFramework;
using ObscurusDebuggerTools;

public static class Program
{
    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    const int SW_HIDE = 0;
    const int SW_SHOW = 5;
    
    [STAThread] static void Main(string[] args)
    {
        if (!Debug.DebugMode)
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
        }
        using var game = new MainGame();
        game.Run();
    }
}