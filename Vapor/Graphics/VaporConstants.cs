namespace Vapor
{
    using SharpDX;
    using System.Runtime.InteropServices;

    /// <summary>
    /// The constants passed to (practically) all shaders.
    /// Look at common.hlsl for the GPU equivalent.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct VaporConstants
    {
        public Matrix Model;
        public Matrix View;
        public Matrix Projection;
        public Matrix ModelView;
        public Matrix ModelViewProjection;
    }
}
