namespace Vapor
{
    public class Component : VaporObject
    {
        public bool Enabled { get; set; }

        public SceneObject SceneObject { get; set; }

        public Transform Transform
        {
            get
            {
                return SceneObject.Transform;
            }
        }

        public Component() : base("Component")
        {
        }

        public Component(string name) : base(name)
        {
        }

        /// <summary>
        /// Automatically called when this <see cref="Component"/> is added to a <see cref="SceneObject"/>.
        /// </summary>
        public virtual void AddedToSceneObject()
        {

        }

        /// <summary>
        /// Automatically called when the <see cref="SceneObject"/> this <see cref="Component"/> belongs to is added to a <see cref="Vapor.Scene"/>.
        /// </summary>
        public virtual void AddedToScene()
        {

        }

        public virtual void Draw(Camera camera)
        {

        }
    }
}
