namespace Hayao
{
    using Vapor;

    public class Game : Application
    {
        private Material material;
        private Mesh triangleMesh;

        public Game() : base()
        {
            material = new Material("vertexShader.hlsl", "pixelShader.hlsl");
            material.Set();

            triangleMesh = Mesh.CreateTriangle();
        }

        protected override void Draw()
        {
            Clear(new SharpDX.Color(32, 103, 178));

            triangleMesh.Draw(material);

            base.Draw();
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