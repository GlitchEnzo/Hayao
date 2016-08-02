namespace Vapor
{
    using SharpDX;

    public class Camera : Component
    {
        public Matrix ProjectionMatrix { get; set; }

        public Color BackgroundColor { get; set; }

        /**
        * The angle, in degrees, of the field of view of the Camera.  Defaults to 45.
        */
        public float FieldOfView { get; set; } = 45.0f;

        /**
         * The aspect ratio (width/height) of the Camera.  Defaults to the GL viewport dimensions.
         */
        public float AspectRatio { get; set; }

        /**
         * The distance to the near clipping plane of the Camera.  Defaults to 0.1.
         */
        public float NearClipPlane { get; set; } = 0.1f;

        /**
         * The distance to the far clipping plane of the Camera.  Defaults to 1000.
         */
        public float FarClipPlane { get; set; } = 1000.0f;


        public Camera() : base("Camera")
        {
            // NOTE: Can NOT do anything with Transform in the constructor, since
            //       it is not yet attached to a SceneObject with a Transform.
            //       Must do it in AddedToSceneObject...

            AspectRatio = Application.Width / Application.Height;
            ProjectionMatrix = Matrix.PerspectiveFovLH(FieldOfView.ToRadians(), AspectRatio, NearClipPlane, FarClipPlane);
        }

        public override void AddedToSceneObject()
        {
            base.AddedToSceneObject();
        
            // initialize the view matrix
            Transform.Position = new Vector3(0.0f, 0.0f, -10.0f);
            Transform.LookAt(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
        }

        public void Clear()
        {
            // TODO: Only clear the area where this camera renders
            Application.Instance.Clear(BackgroundColor);
        }
    }
}
