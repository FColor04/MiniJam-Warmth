using System;
using MainGameFramework;

public static class Program
{
    [STAThread] static void Main(string[] args)
    {
        using var game = new MainGame();
        game.Run();
    }
}