namespace ReFactory.ExtraMathFunctions
{
    public class ReFactoryExtraMathFunctions
    {
        public static float InverseLerp(float a, float b, float value)
        {
            if (a != b)
            {
                return (value - a) / (b - a);
            }
            else
            {
                return 0.0f;
            }
        }
    }
}
