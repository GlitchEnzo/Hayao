namespace Vapor
{
    using System.Collections.Generic;

    public class SceneObject : VaporObject
    {
        public Transform Transform { get; set; }

        public List<Component> Components { get; set; } = new List<Component>();

        public List<SceneObject> Children { get; set; } = new List<SceneObject>();

        public SceneObject Parent { get; set; }

        public Scene Scene { get; set; }

        public Renderer Renderer { get; private set; }

        public SceneObject(string name) : base(name)
        {
            Transform = new Transform();
            AddComponent(Transform);
        }

        /// <summary>
        /// Automatically called when this <see cref="SceneObject"/> is added to a <see cref="Vapor.Scene"/>.
        /// </summary>
        public virtual void AddedToScene()
        {
            for (int i = 0; i < Components.Count; i++)
            {
                Components[i].AddedToScene();
            }
        }

        /// <summary>
        /// Automatically called when this <see cref="SceneObject"/> is removed from a <see cref="Vapor.Scene"/>.
        /// </summary>
        /// <param name="scene">The <see cref="Vapor.Scene"/> this <see cref="SceneObject"/> was removed from.</param>
        public virtual void RemovedFromScene(Scene scene)
        {

        }

        public virtual T AddComponent<T>() where T : Component, new()
        {
            T component = new T();
            component.SceneObject = this;

            if (component.GetType().IsSubclassOf(typeof(Renderer)))
            {
                Renderer = component as Renderer;
            }

            Components.Add(component);
            component.AddedToSceneObject();

            return component;
        }

        public virtual Component AddComponent(Component component)
        {
            component.SceneObject = this;

            if (component.GetType().IsSubclassOf(typeof(Renderer)))
            {
                Renderer = component as Renderer;
            }

            Components.Add(component);
            component.AddedToSceneObject();

            return component;
        }

        public virtual T GetComponent<T>() where T : Component
        {
            T found = null;
            for (int i = 0; i < Components.Count; i++)
            {
                if (Components[i] is Camera || Components[i].GetType().IsSubclassOf(typeof(T)))
                {
                    found = (T)Components[i];
                    break;
                }
            }
            return found;
        }

        public virtual void RemoveComponent(Component component)
        {
            component.SceneObject = null;

            if (component.GetType().IsSubclassOf(typeof(Renderer)))
            {
                Renderer = null;
            }

            Components.Remove(component);
        }

        public virtual void Update()
        {

        }

        public virtual void Draw(Camera camera)
        {
            for (int i = 0; i < Components.Count; i++)
            {
                Components[i].Draw(camera);
            }
        }

        public static SceneObject CreateCamera()
        {
            SceneObject sceneObject = new SceneObject("Camera");
            sceneObject.AddComponent<Camera>();

            return sceneObject;
        }

        public static SceneObject CreateTriangle()
        {
            SceneObject sceneObject = new SceneObject("Triangle");
            MeshRenderer meshRenderer = sceneObject.AddComponent<MeshRenderer>();
            meshRenderer.Mesh = Mesh.CreateTriangle();

            return sceneObject;
        }

        public static SceneObject CreateCube()
        {
            SceneObject sceneObject = new SceneObject("Cube");
            MeshRenderer meshRenderer = sceneObject.AddComponent<MeshRenderer>();
            meshRenderer.Mesh = Mesh.CreateCube();

            return sceneObject;
        }
    }
}
