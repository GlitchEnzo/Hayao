namespace Vapor
{
    using SharpDX;
    using System.Collections.Generic;

    public class Scene : VaporObject
    {
        List<SceneObject> SceneObjects = new List<SceneObject>();

        List<Camera> Cameras = new List<Camera>();

        public bool Paused { get; set; }

        //private VaporConstants constants = new VaporConstants();

        public Scene() : base("Scene")
        {
        }

        public Scene(string sceneName) : base("Scene: " + sceneName)
        {
        }

        public void AddSceneObject(SceneObject sceneObject)
        {
            sceneObject.Scene = this;

            foreach (var component in sceneObject.Components)
            {
                // Check if sceneObject contains Camera component.  Add to camera list if it does.
                if (component is Camera || component.GetType().IsSubclassOf(typeof(Camera)))
                {
                    Cameras.Add(component as Camera);
                }

                // TODO: Check if sceneObject contains Light component.  Add to light list if it does.
            }

            SceneObjects.Add(sceneObject);

            sceneObject.AddedToScene();
        }

        public void RemoveSceneObject(SceneObject sceneObject)
        {
            sceneObject.Scene = null;

            foreach (var component in sceneObject.Components)
            {
                // Check if sceneObject contains Camera component. Remove camera list if it does.
                if (component is Camera || component.GetType().IsSubclassOf(typeof(Camera)))
                {
                    Cameras.Remove(component as Camera);
                }

                // TODO: Check if sceneObject contains Light component.  Remove from light list if it does.
            }

            SceneObjects.Remove(sceneObject);

            sceneObject.RemovedFromScene(this);
        }

        public void Update()
        {
            if (!Paused)
            {
                for (int i = 0; i < SceneObjects.Count; i++)
                {
                    SceneObjects[i].Update();
                }
            }
        }

        public void Draw()
        {
            for (int j = 0; j < Cameras.Count; j++)
            {
                Cameras[j].Clear();

                for (int i = 0; i < SceneObjects.Count; i++)
                {
                    // Set the view & projection matrix on each renderer
                    //if (SceneObjects[i].Renderer != null)
                    //{
                    //    var viewMatrix = Cameras[j].Transform.ModelMatrix;
                    //    viewMatrix.Invert();
                    //    //constants.ViewMatrix = viewMatrix;
                    //    //constants.ProjectionMatrix = Cameras[j].ProjectionMatrix;
                    //    constants.ViewMatrix = Matrix.Identity;
                    //    constants.ProjectionMatrix = Matrix.Identity;
                    //    SceneObjects[i].Renderer.Material.SetConstantBuffer("VaporConstants", constants);
                    //}

                    SceneObjects[i].Draw(Cameras[j]);
                }
            }
        }
    }
}
