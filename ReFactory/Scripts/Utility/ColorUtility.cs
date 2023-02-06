using System;
using Utility;
using Color = Microsoft.Xna.Framework.Color;

namespace ReFactory.Utility;

public static class ColorUtility
{
    public static Color ToolbarGrey => FromHex("#333333");
    public static Color ToolbarSilver => FromHex("#909090");
    public static Color FillBarYellow => FromHex("#ffaa00");

    public static Color ToXnaColor(this System.Drawing.Color color) => new (color.R, color.G, color.B, color.A);
    public static Color FromHex(string hex) => System.Drawing.ColorTranslator.FromHtml(hex).ToXnaColor();
    
    public static Color HSVColor(Rotation hue, float saturation, float value)
    {
        int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
        double f = hue / 60 - Math.Floor(hue / 60);

        value = value * 255;
        int v = Convert.ToInt32(value);
        int p = Convert.ToInt32(value * (1 - saturation));
        int q = Convert.ToInt32(value * (1 - f * saturation));
        int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

        if (hi == 0)
            return new Color(v, t, p, 255);
        if (hi == 1)
            return new Color(q, v, p, 255);
        if (hi == 2)
            return new Color(p, v, t, 255);
        if (hi == 3)
            return new Color(p, q, v, 255);
        if (hi == 4)
            return new Color(t, p, v, 255);
        return new Color(v, p, q, 255);
    }
}