namespace Vapor
{
    using SharpDX;

    public class Renderer : Component
    {
        public Material Material { get; set; }

        private VaporConstants constants = new VaporConstants();

        public override void Draw(Camera camera)
        {
            Material.Set();

            Matrix translationMatrix = Matrix.Identity;
            translationMatrix.TranslationVector = camera.Transform.ModelMatrix.TranslationVector;

            constants.Model = SceneObject.Transform.ScaledModelMatrix;
            constants.View = camera.ViewMatrix;
            //constants.View.Invert();
            constants.Projection = camera.ProjectionMatrix;
            constants.ModelView = SceneObject.Transform.ModelMatrix * constants.View;
            constants.ModelViewProjection = SceneObject.Transform.ModelMatrix * constants.View * camera.ProjectionMatrix;

            constants.ModelViewProjection.Transpose();

            Material.UpdateConstantBuffer("VaporConstants", constants);
        }
    }
}
