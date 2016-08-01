namespace Vapor
{
    using SharpDX;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct VaporModelConstants
    {
        public Matrix ModelViewMatrix;
        public Matrix ModelMatrix;
    }
}
