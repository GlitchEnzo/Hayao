namespace Vapor
{
    public class MeshRenderer : Renderer
    {
        public Mesh Mesh { get; set; }

        public override void Draw()
        {
            Material.Set();
            //Material.SetMatrix("uModelViewMatrix", SceneObject.Transform.modelMatrix);
            //Material.SetMatrix("uModelMatrix", SceneObject.Transform.ScaledModelMatrix);
            Mesh.Draw(Material);
        }
    }
}
