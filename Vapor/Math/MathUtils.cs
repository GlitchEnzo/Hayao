namespace Vapor
{
    using SharpDX;

    public static class MathUtils
    {
        public static float ToRads = (float)System.Math.PI / 180.0f;

        public static float ToDegs = 180.0f / (float)System.Math.PI;

        public static float ToRadians(this float degrees)
        {
            return degrees * ToRads;
        }

        public static float ToDegrees(this float radians)
        {
            return radians * ToDegs;
        }

        public static void DumpRowMajor(this Matrix matrix)
        {
            Log.Info(@"Row Major:
{0}, {1}, {2}, {3}
{4}, {5}, {6}, {7}
{8}, {9}, {10}, {11}
{12}, {13}, {14}, {15}", 
                       matrix[0], matrix[1], matrix[2], matrix[3], 
                       matrix[4], matrix[5], matrix[6], matrix[7], 
                       matrix[8], matrix[9], matrix[10], matrix[11], 
                       matrix[12], matrix[13], matrix[14], matrix[15]);
        }

        public static void DumpColumnMajor(this Matrix matrix)
        {
            Log.Info(@"Column Major:
{0}, {4}, {8}, {12}
{1}, {5}, {9}, {13}
{2}, {6}, {10}, {14}
{3}, {7}, {11}, {15}",
                       matrix[0], matrix[1], matrix[2], matrix[3],
                       matrix[4], matrix[5], matrix[6], matrix[7],
                       matrix[8], matrix[9], matrix[10], matrix[11],
                       matrix[12], matrix[13], matrix[14], matrix[15]);
        }
    }
}
