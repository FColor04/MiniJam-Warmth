using Microsoft.Xna.Framework;

namespace Utility;

public struct Rotation
{
    public static readonly Rotation Up = new (0);
    public static readonly Rotation Right = new (90);
    public static readonly Rotation Down = new (180);
    public static readonly Rotation Left = new (270);
    private float _absoluteRotation;

    public Rotation(float degrees)
    {
        _absoluteRotation = 0;
        Degrees = degrees;
    }

    public float Degrees
    {
        get
        {
            while (_absoluteRotation < 0)
            {
                _absoluteRotation += 360;
            }
            _absoluteRotation %= 360;
            return _absoluteRotation;
        }
        set
        {
            var newValue = value;
            while (newValue < 0)
            {
                newValue += 360;
            }
            newValue %= 360;
            _absoluteRotation = newValue;
        }
    }
    
    public float Radians => MathHelper.ToRadians(Degrees);

    public static Rotation operator +(Rotation a) => a;
    public static Rotation operator -(Rotation a) => new (-a.Degrees);
    
    public static Rotation operator +(Rotation a, Rotation b) => new (a.Degrees + b.Degrees);
    public static float operator +(Rotation a, float b) => new Rotation(a.Degrees + b).Degrees;
    public static float operator +(float b, Rotation a) => new Rotation(a.Degrees + b).Degrees;
    
    public static Rotation operator -(Rotation a, Rotation b) => new (a.Degrees - b.Degrees);
    public static float operator -(Rotation a, float b) => new Rotation(a.Degrees - b).Degrees;
    public static float operator -(float b, Rotation a) => new Rotation(a.Degrees - b).Degrees;

    public static Rotation operator *(Rotation a, Rotation b) => new (a.Degrees * b.Degrees);
    public static float operator *(Rotation a, float b) => new Rotation(a.Degrees * b).Degrees;
    public static float operator *(float b, Rotation a) => new Rotation(a.Degrees * b).Degrees;
    
    public static Rotation operator /(Rotation a, Rotation b) => new (a.Degrees / b.Degrees);
    public static float operator /(Rotation a, float b) => new Rotation(a.Degrees / b).Degrees;
    public static float operator /(float b, Rotation a) => new Rotation(a.Degrees / b).Degrees;
}