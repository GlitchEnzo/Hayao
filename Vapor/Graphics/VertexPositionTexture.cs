namespace Vapor
{
    using SharpDX;
    using SharpDX.Direct3D11;
    using SharpDX.DXGI;

    public struct VertexPositionTexture : IVertexType
    {
        public Vector3 Position;

        public Vector2 UV;

        public VertexPositionTexture(Vector3 position, Vector2 uv)
        {
            Position = position;
            UV = uv;
        }

        public InputElement[] GetInputElements()
        {
            // TODO: Optimize it by caching the array?
            return new InputElement[]
            {
                new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0),
                new InputElement("TEXCOORD", 0, Format.R32G32_Float, 12, 0)
            };
        }
    }
}
