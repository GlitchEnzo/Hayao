namespace Hayao
{
    using Vapor;

    public class CameraSlider : Component
    {
        public float Max = 12;
        public float Min = 4;

        private bool forward = true;

        public override void Update()
        {
            base.Update();

            var posZ = Transform.Position.Z;
            var delta = Time.DeltaTime * 8;

            if (forward)
            {
                posZ += delta;
                if (posZ >= Max)
                {
                    forward = false;
                }
            }
            else
            {
                posZ -= delta;
                if (posZ <= Min)
                {
                    forward = true;
                }
            }

            Transform.Position = new SharpDX.Vector3(Transform.Position.X, Transform.Position.Y, posZ);
        }
    }
}
