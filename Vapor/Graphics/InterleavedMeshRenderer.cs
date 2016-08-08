namespace Vapor
{
    public class InterleavedMeshRenderer<T> : Renderer where T : struct, IVertexType
    {
        public InterleavedMesh<T> Mesh { get; set; }

        public override void Draw(Camera camera)
        {
            base.Draw(camera);

            Mesh.Draw(Material);
        }
    }
}
