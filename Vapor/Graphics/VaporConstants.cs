namespace Vapor
{
    using SharpDX;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct VaporConstants
    {
        public Matrix ViewMatrix;
        public Matrix ProjectionMatrix;
    }
}
