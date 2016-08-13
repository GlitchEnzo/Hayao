namespace Vapor
{
    using SharpDX;
    using SharpDX.Direct3D11;
    using SharpDX.DXGI;

    public struct VertexPositionTextureNormal : IVertexType
    {
        public Vector4 Position;

        public Vector3 TextureCoordinates;

        public Vector3 Normal;

        public VertexPositionTextureNormal(Vector4 position, Vector3 texCoords, Vector3 normal)
        {
            Position = position;
            TextureCoordinates = texCoords;
            Normal = normal;
        }

        public InputElement[] GetInputElements()
        {
            // TODO: Optimize it by caching the array?
            return new InputElement[]
            {
                new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0), // 4 bytes * 4 = 16 bytes
                new InputElement("TEXCOORD", 0, Format.R32G32B32_Float, 16, 0),   // 4 bytes * 3 = 12 bytes
                new InputElement("NORMAL", 0, Format.R32G32B32_Float, 28, 0)
            };
        }
    }
}
