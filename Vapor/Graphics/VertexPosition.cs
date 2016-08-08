namespace Vapor
{
    using SharpDX;
    using SharpDX.Direct3D11;
    using SharpDX.DXGI;

    public struct VertexPosition : IVertexType
    {
        public Vector3 Position;

        public VertexPosition(Vector3 position)
        {
            Position = position;
        }

        public InputElement[] GetInputElements()
        {
            // TODO: Optimize it by caching the array?
            return new InputElement[]
            {
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0)
            };
        }
    }
}
