using System;
using System.Runtime.InteropServices;
using MainGameFramework;
using ReFactory.Debugger;

public static class Program
{
    [DllImport("kernel32.dll")]
    static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    const int SwHide = 0;
    const int SwShow = 5;
    
    [STAThread] static void Main(string[] args)
    {
        if (!Debug.debugMode)
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SwHide);
        }
        using var game = new MainGame();
        game.Run();
    }
}