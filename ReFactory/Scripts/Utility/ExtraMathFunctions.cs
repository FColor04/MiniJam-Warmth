using System;

namespace ReFactory.Utility
{
    public class ExtraMathFunctions
    {
        public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0) return min;
            if (value.CompareTo(max) > 0) return max;
            return value;
        }
        public static float InverseLerp(float a, float b, float value)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (a == b) return 0.0f;
            return (value - a) / (b - a);
        }

        public static float InverseLerpClamped(float a, float b, float value)
        {
            if (a == b) return 0.0f;
            float result = (value - a) / (b - a);
            return Clamp(result, 0.0f, 1.0f);
        }

        #region Simplex Noise
        public class SimplexNoise
        {
            private const int GradientSizeTable = 256;
            private readonly float[] _gradients = new float[GradientSizeTable * 3];
            private readonly int[] _perm = new int[GradientSizeTable];

            public SimplexNoise(int seed)
            {
                Random rnd = new Random(seed);

                for (int i = 0; i < GradientSizeTable; i++)
                {
                    float z = 1f - 2f * (float)rnd.NextDouble();
                    float r = (float)Math.Sqrt(1f - z * z);
                    float theta = 2 * (float)Math.PI * (float)rnd.NextDouble();
                    _gradients[i * 3] = r * (float)Math.Cos(theta);
                    _gradients[i * 3 + 1] = r * (float)Math.Sin(theta);
                    _gradients[i * 3 + 2] = z;
                }

                for (int i = 0; i < GradientSizeTable; i++)
                {
                    _perm[i] = i;
                }

                for (int i = 0; i < GradientSizeTable; i++)
                {
                    int j = rnd.Next(GradientSizeTable);
                    int k = _perm[i];
                    _perm[i] = _perm[j];
                    _perm[j] = k;
                }
            }

            public float Noise(float x, float y, float z)
            {
                float n0, n1, n2, n3;
                float s = (x + y + z) / 3;
                int i = FastFloor(x + s);
                int j = FastFloor(y + s);
                int k = FastFloor(z + s);

                float t = (i + j + k) / 6f;
                float X0 = i - t;
                float Y0 = j - t;
                float Z0 = k - t;

                float x0 = x - X0;
                float y0 = y - Y0;
                float z0 = z - Z0;

                int i1, j1, k1;
                int i2, j2, k2;

                if (x0 >= y0)
                {
                    if (y0 >= z0)
                    {
                        i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 1; k2 = 0;
                    }
                    else if (x0 >= z0)
                    {
                        i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 0; k2 = 1;
                    }
                    else
                    {
                        i1 = 0; j1 = 0; k1 = 1; i2 = 1; j2 = 0; k2 = 1;
                    }
                }
                else
                {
                    if (y0 < z0)
                    {
                        i1 = 0; j1 = 0; k1 = 1; i2 = 0; j2 = 1; k2 = 1;
                    }
                    else if (x0 < z0)
                    {
                        i1 = 0; j1 = 1; k1 = 0; i2 = 0; j2 = 1; k2 = 1;
                    }
                    else
                    {
                        i1 = 0; j1 = 1; k1 = 0; i2 = 1; j2 = 1; k2 = 0;
                    }
                }

                float x1 = x0 - i1 + (1f / 6f);
                float y1 = y0 - j1 + (1f / 6f);
                float z1 = z0 - k1 + (1f / 6f);
                float x2 = x0 - i2 + (2f / 6f);
                float y2 = y0 - j2 + (2f / 6f);
                float z2 = z0 - k2 + (2f / 6f);
                float x3 = x0 - 1f + (3f / 6f);
                float y3 = y0 - 1f + (3f / 6f);
                float z3 = z0 - 1f + (3f / 6f);

                int ii = i & 0xff;
                int jj = j & 0xff;
                int kk = k & 0xff;

                float t0 = 0.6f - x0 * x0 - y0 * y0 - z0 * z0;
                if (t0 < 0) n0 = 0.0f;
                else
                {
                    int gi0 = _perm[ii + _perm[jj + _perm[kk]]] % 12;
                    t0 *= t0;
                    n0 = t0 * t0 * Dot(_gradients, gi0, x0, y0, z0);
                }

                float t1 = 0.6f - x1 * x1 - y1 * y1 - z1 * z1;
                if (t1 < 0) n1 = 0.0f;
                else
                {
                    int gi1 = _perm[ii + i1 + _perm[jj + j1 + _perm[kk + k1]]] % 12;
                    t1 *= t1;
                    n1 = t1 * t1 * Dot(_gradients, gi1, x1, y1, z1);
                }

                float t2 = 0.6f - x2 * x2 - y2 * y2 - z2 * z2;
                if (t2 < 0) n2 = 0.0f;
                else
                {
                    int gi2 = _perm[ii + i2 + _perm[jj + j2 + _perm[kk + k2]]] % 12;
                    t2 *= t2;
                    n2 = t2 * t2 * Dot(_gradients, gi2, x2, y2, z2);
                }

                float t3 = 0.6f - x3 * x3 - y3 * y3 - z3 * z3;
                if (t3 < 0) n3 = 0.0f;
                else
                {
                    int gi3 = _perm[ii + 1 + _perm[jj + 1 + _perm[kk + 1]]] % 12;
                    t3 *= t3;
                    n3 = t3 * t3 * Dot(_gradients, gi3, x3, y3, z3);
                }
                return 32.0f * (n0 + n1 + n2 + n3);
            }

            private static float Dot(float[] g, int i, float x, float y, float z)
            {
                return g[i * 3] * x + g[i * 3 + 1] * y + g[i * 3 + 2] * z;
            }

            private static int FastFloor(float x)
            {
                int xi = (int)x;
                return x < xi ? xi - 1 : xi;
            }
        }
        #endregion




//=========End of File=========//
    }
}