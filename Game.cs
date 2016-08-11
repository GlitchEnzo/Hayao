namespace Hayao
{
    using Vapor;

    public class Game : Application
    {
        private Material material;
        private InterleavedMesh<VertexPositionTexture> triangleMesh;

        public Game() : base()
        {
            material = new Material("vertexShader.hlsl", "pixelShader.hlsl");
            material.Set();

            triangleMesh = InterleavedMesh<VertexPositionTexture>.CreateTriangle();
        }

        protected override void Loop()
        {
            Clear(new SharpDX.Color(32, 103, 178));

            triangleMesh.Draw(material);

            base.Loop();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                triangleMesh.Dispose();
                material.Dispose();
            }
        }
    }
}