using System;
using Color = Microsoft.Xna.Framework.Color;

namespace ReFactory.Utility;

public static class ColorUtility
{
    public static Color ToolbarGrey => FromHex("#333333");
    public static Color ToolbarSilver => FromHex("#909090");
    public static Color FillBarYellow => FromHex("#ffaa00");

    public static Color ToXnaColor(this System.Drawing.Color color) => new (color.R, color.G, color.B, color.A);
    public static Color FromHex(string hex) => System.Drawing.ColorTranslator.FromHtml(hex).ToXnaColor();
    
    public static void HsvToRgb(float h, float S, float V, out int r, out int g, out int b)
    {    
        float H = h;
        while (H < 0) { H += 360; };
        while (H >= 360) { H -= 360; };
        float R, G, B;
        if (V <= 0)
        { R = G = B = 0; }
        else if (S <= 0)
        {
            R = G = B = V;
        }
        else
        {
            float hf = H / 60.0f;
            int i = (int)Math.Floor(hf);
            float f = hf - i;
            float pv = V * (1 - S);
            float qv = V * (1 - S * f);
            float tv = V * (1 - S * (1 - f));
            switch (i)
            {

                // Red is the dominant color

                case 0:
                    R = V;
                    G = tv;
                    B = pv;
                    break;

                // Green is the dominant color

                case 1:
                    R = qv;
                    G = V;
                    B = pv;
                    break;
                case 2:
                    R = pv;
                    G = V;
                    B = tv;
                    break;

                // Blue is the dominant color

                case 3:
                    R = pv;
                    G = qv;
                    B = V;
                    break;
                case 4:
                    R = tv;
                    G = pv;
                    B = V;
                    break;

                // Red is the dominant color

                case 5:
                    R = V;
                    G = pv;
                    B = qv;
                    break;

                // Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

                case 6:
                    R = V;
                    G = tv;
                    B = pv;
                    break;
                case -1:
                    R = V;
                    G = pv;
                    B = qv;
                    break;

                // The color is not defined, we should throw an error.

                default:
                    //LFATAL("i Value error in Pixel conversion, Value is %d", i);
                    R = G = B = V; // Just pretend its black/white
                    break;
            }
        }
        r = ClampColor((int)(R * 255.0));
        g = ClampColor((int)(G * 255.0));
        b = ClampColor((int)(B * 255.0));
    }

    /// <summary>
    /// Clamp a value to 0-255
    /// </summary>
    static int ClampColor(int i)
    {
        if (i < 0) return 0;
        if (i > 255) return 255;
        return i;
    }

    public static void HsvToRgb(float h, float s, float v, out Color color)
    {
        HsvToRgb(h, s, v, out var r, out var g, out var b);
        color = new Color(r, g, b);
    }
}