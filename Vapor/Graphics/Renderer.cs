namespace Vapor
{
    public class Renderer : Component
    {
        public Material Material { get; set; }

        private VaporConstants constants = new VaporConstants();

        public override void Draw(Camera camera)
        {
            Material.Set();

            constants.Model = SceneObject.Transform.ScaledModelMatrix;
            constants.View = camera.ViewMatrix;
            constants.Projection = camera.ProjectionMatrix;
            constants.ModelView = SceneObject.Transform.ModelMatrix * camera.ViewMatrix;
            constants.ModelViewProjection = SceneObject.Transform.ModelMatrix * camera.ViewMatrix * camera.ProjectionMatrix;

            constants.ModelViewProjection.Transpose();

            Material.SetConstantBuffer("VaporConstants", constants);
        }
    }
}
