namespace Vapor
{
    using SharpDX;

    public class Transform : Component
    {
        public Matrix ModelMatrix { get; set; }

        private Matrix ScaleMatrix { get; set; }

        public Matrix ScaledModelMatrix
        {
            get
            {
                // TODO: Optimize this to only do the multiplication when needed.
                //return this.modelMatrix * this.scaleMatrix;
                return Matrix.Multiply(this.ModelMatrix, this.ScaleMatrix);
            }
        }

        public Quaternion Rotation { get; set; }

        private Vector3 eulerAngles;

        public Vector3 EulerAngles
        {
            get
            {
                // TODO: Actually calculate the angles instead of using old values.
                return eulerAngles;
            }
            set
            {
                eulerAngles = value;
                Rotation = Quaternion.RotationYawPitchRoll(eulerAngles[0], eulerAngles[1], eulerAngles[2]);

                Matrix matrix = Matrix.RotationQuaternion(Rotation);
                matrix.TranslationVector = Position;
                ModelMatrix = matrix;
            }
        }

        private Vector3 scale;

        public Vector3 Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
                ScaleMatrix = Matrix.Scaling(scale);
            }
        }

        public Vector3 Position
        {
            get
            {
                return new Vector3(ModelMatrix[12], ModelMatrix[13], ModelMatrix[14]);
            }
            set
            {
                for (var i = 0; i < SceneObject.Children.Count; i++)
                {
                    var child = SceneObject.Children[i];

                    // TODO: Set the position in local space, not world space.
                    child.Transform.Position = child.Transform.LocalPosition;
                    child.Transform.Position += value;
                }

                Matrix matrix = ModelMatrix;
                matrix[12] = value[0];
                matrix[13] = value[1];
                matrix[14] = value[2];
                ModelMatrix = matrix;

                // Change RigidBody position as well, if there is one
            }
        }

        public Vector3 LocalPosition
        {
            get
            {
                // if there is no parent, the local position is the world position
                Vector3 local = Position;
                if (SceneObject.Parent != null)
                {
                    // TODO: Use local space, not world space.
                    local -= SceneObject.Parent.Transform.Position;
                }
                return local;
            }
            set
            {
                if (SceneObject.Parent != null)
                {
                    // TODO: Use local space, not world space.
                    Position = SceneObject.Parent.Transform.Position;
                    Position += value;
                }
                else
                {
                    Position = value;
                }
            }
        }

        public Vector3 Right
        {
            get
            {
                return new Vector3(ModelMatrix[0], ModelMatrix[1], ModelMatrix[2]);
            }
            set
            {
                Matrix matrix = ModelMatrix;
                matrix[0] = value[0];
                matrix[1] = value[1];
                matrix[2] = value[2];
                ModelMatrix = matrix;

                // TODO: Recalc forward and up
            }
        }

        public Vector3 Up
        {
            get
            {
                return new Vector3(ModelMatrix[4], ModelMatrix[5], ModelMatrix[6]);
            }
            set
            {
                Matrix matrix = ModelMatrix;
                matrix[4] = value[0];
                matrix[5] = value[1];
                matrix[6] = value[2];
                ModelMatrix = matrix;

                // TODO: Recalc forward and up
            }
        }

        public Vector3 Forward
        {
            get
            {
                return -new Vector3(ModelMatrix[8], ModelMatrix[9], ModelMatrix[10]);
            }
            set
            {
                Matrix matrix = ModelMatrix;
                matrix[8] = -value[0];
                matrix[9] = -value[1];
                matrix[10] = -value[2];
                ModelMatrix = matrix;

                // TODO: Recalc forward and up
            }
        }

        public Transform() : base("Transform")
        {
            this.ModelMatrix = new Matrix();
            this.Rotation = new Quaternion();
            this.EulerAngles = new Vector3();
            this.Scale = new Vector3(1.0f, 1.0f, 1.0f);
            this.ScaleMatrix = new Matrix();
        }

        public void LookAt(Vector3 targetPosition, Vector3 worldUp)
        {
            // TODO: worldUp should only be a hint, not "solid"
            ModelMatrix = Matrix.LookAtLH(Position, targetPosition, worldUp);
        }

        public void Rotate(Vector3 axis, float angle)
        {
            ModelMatrix = ModelMatrix * Matrix.RotationAxis(axis, angle);
        }

        public void RotateLocalX(float angle)
        {
            Rotate(Right, angle);
        }

        public void RotateLocalY(float angle)
        {
            Rotate(Up, angle);
        }
    }
}
