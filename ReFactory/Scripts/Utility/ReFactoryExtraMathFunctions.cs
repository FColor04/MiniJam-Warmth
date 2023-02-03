namespace ReFactory.ExtraMathFunctions
{
    public class ReFactoryExtraMathFunctions
    {
        public static float InverseLerp(float a, float b, float value)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (a == b) return 0.0f;
            return (value - a) / (b - a);
        }
        
        public static float InverseLerpClamped(float a, float b, float value)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (a == b) return 0.0f;
            return (value - a) / (b - a);
        }
    }
}
