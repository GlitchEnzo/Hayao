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
                return Matrix.Multiply(ModelMatrix, ScaleMatrix);
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
            ModelMatrix = Matrix.Identity;
            Rotation = Quaternion.Identity;
            EulerAngles = new Vector3();
            Scale = new Vector3(1.0f, 1.0f, 1.0f);
            ScaleMatrix = Matrix.Identity;
        }

        /// <summary>
        /// https://github.com/sharpdx/SharpDX/blob/master/Source/SharpDX.Mathematics/Matrix.cs
        /// </summary>
        /// <param name="eye"></param>
        /// <param name="target"></param>
        /// <param name="up"></param>
        /// <param name="result"></param>
        private static void LookAtLH(ref Vector3 eye, ref Vector3 target, ref Vector3 up, out Matrix result)
        {
            Vector3 xaxis, yaxis, zaxis;

            // calc forward vector
            Vector3.Subtract(ref target, ref eye, out zaxis);
            zaxis.Normalize();

            // calc right vector
            Vector3.Cross(ref up, ref zaxis, out xaxis);
            xaxis.Normalize();

            // calc up vector
            Vector3.Cross(ref zaxis, ref xaxis, out yaxis);

            result = Matrix.Identity;
            result.M11 = xaxis.X; result.M21 = xaxis.Y; result.M31 = xaxis.Z;
            result.M12 = yaxis.X; result.M22 = yaxis.Y; result.M32 = yaxis.Z;
            result.M13 = zaxis.X; result.M23 = zaxis.Y; result.M33 = zaxis.Z;

            result.TranslationVector = eye;
        }


        /// <summary>
        /// "Looks" from the eyePosition to the targetPosition, using the given worldUp as a reference.
        /// </summary>
        /// <remarks>
        /// This does NOT calculate the matrix as a "view" matrix.  This means that you don't need to invert it and that the translation vector is correct (no dot product).
        /// </remarks>
        /// <param name="eyePosition"></param>
        /// <param name="targetPosition"></param>
        /// <param name="worldUp"></param>
        /// <returns></returns>
        private static Matrix LookAtLH(Vector3 eyePosition, Vector3 targetPosition, Vector3 worldUp)
        {
            Matrix result;
            LookAtLH(ref eyePosition, ref targetPosition, ref worldUp, out result);
            return result;
        }

        public void LookAt(Vector3 eyePosition, Vector3 targetPosition, Vector3 worldUp)
        {
            //ModelMatrix = Matrix.LookAtLH(eyePosition, targetPosition, worldUp);
            //ModelMatrix.Invert();

            Matrix result;
            LookAtLH(ref eyePosition, ref targetPosition, ref worldUp, out result);
            ModelMatrix = result;
        }

        public void LookAt(Vector3 targetPosition, Vector3 worldUp)
        {
            //ModelMatrix = Matrix.LookAtLH(Position, targetPosition, worldUp);
            //ModelMatrix.Invert();

            //Matrix result;
            //Vector3 position = Position;
            //LookAtLH(ref position, ref targetPosition, ref worldUp, out result);
            //ModelMatrix = result;

            LookAt(Position, targetPosition, worldUp);
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
