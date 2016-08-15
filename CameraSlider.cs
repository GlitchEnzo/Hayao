namespace Hayao
{
    using SharpDX; // for Vector3
    using Vapor;

    public enum Direction
    {
        X,
        Y,
        Z
    }

    public class CameraSlider : Component
    {
        public float Zmax = 12;
        public float Zmin = 4;

        public float Xmax = 2;
        public float Xmin = -2;

        private bool forward = true;

        public Direction Direction = Direction.X;

        public override void Update()
        {
            base.Update();

            var delta = Time.DeltaTime * 2;

            switch (Direction)
            {
                case Direction.X:
                    var posX = Transform.Position.X;

                    if (forward)
                    {
                        posX += delta;
                        if (posX >= Xmax)
                        {
                            posX = Xmax;
                            forward = false;
                        }
                    }
                    else
                    {
                        posX -= delta;
                        if (posX <= Xmin)
                        {
                            posX = Xmin;
                            forward = true;
                        }
                    }

                    Transform.Position = new Vector3(posX, Transform.Position.Y, Transform.Position.Z);

                    Transform.LookAt(Transform.Position, new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));

                    break;

                case Direction.Z:
                    var posZ = Transform.Position.Z;

                    if (forward)
                    {
                        posZ += delta;
                        if (posZ >= Zmax)
                        {
                            posZ = Zmax;
                            forward = false;
                        }
                    }
                    else
                    {
                        posZ -= delta;
                        if (posZ <= Zmin)
                        {
                            posZ = Zmin;
                            forward = true;
                        }
                    }

                    Transform.Position = new Vector3(Transform.Position.X, Transform.Position.Y, posZ);

                    break;
            }
        }
    }
}
