namespace Vapor
{
    public class MeshRenderer : Renderer
    {
        public Mesh Mesh { get; set; }

        private VaporModelConstants constants = new VaporModelConstants();

        public override void Draw()
        {
            Material.Set();
            ////Material.SetMatrix("ModelViewMatrix", SceneObject.Transform.ModelMatrix);
            //Material.SetMatrix("ModelMatrix", SceneObject.Transform.ScaledModelMatrix);
            constants.ModelViewMatrix = SceneObject.Transform.ModelMatrix;
            constants.ModelMatrix = SceneObject.Transform.ScaledModelMatrix;
            Material.SetConstantBuffer("VaporModelConstants", constants);
            Mesh.Draw(Material);
        }
    }
}
