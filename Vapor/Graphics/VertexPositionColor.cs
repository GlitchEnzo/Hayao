namespace Vapor
{
    using SharpDX;
    using SharpDX.Direct3D11;
    using SharpDX.DXGI;

    public struct VertexPositionColor : IVertexType
    {
        public Vector3 Position;

        public Color Color;

        public VertexPositionColor(Vector3 position, Color color)
        {
            Position = position;
            Color = color;
        }

        public InputElement[] GetInputElements()
        {
            // TODO: Optimize it by caching the array?
            return new InputElement[]
            {
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                new InputElement("COLOR", 0, Format.R8G8B8A8_UNorm, 12, 0)
            };
        }
    }
}
