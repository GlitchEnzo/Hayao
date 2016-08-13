namespace Hayao
{
    using SharpDX; // needed for math (Vector, Matrix, Color, etc)
    using Vapor;

    class Program
    {
        static void Main(string[] args)
        {
            //UseGame();
            UseVapor();
        }

        static void UseGame()
        {
            using (Game game = new Game())
            {
                game.Run();
            }
        }

        static void UseVapor()
        {
            Application.Instance.WindowTitle = "Hayao";

            ObjLoader.LoadObj("Models//cube.obj");

            Texture2D cloudTexture = Texture2D.FromFile("Textures//cloud.jpg");     

            // create the scene
            Scene scene = new Scene("MyScene");

            // create a simple camera
            SceneObject cameraObject = SceneObject.CreateCamera();
            cameraObject.Transform.Position = new Vector3(0.0f, 0.0f, -5.0f);
            cameraObject.Transform.LookAt(new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 1.0f, 0.0f));
            Camera camera = cameraObject.GetComponent<Camera>();
            camera.BackgroundColor = new Color(32, 103, 178);
            cameraObject.AddComponent<CameraSlider>();
            scene.AddSceneObject(cameraObject);

            // create the material from loaded shaders
            Material material = new Material("Shaders//vertexShader.hlsl", "Shaders//pixelShader.hlsl");
            material.SetTexture("ShaderTexture", cloudTexture);

            if (false)
            {
                // create a simple triangle
                SceneObject triangle = SceneObject.CreateTriangle();
                triangle.Renderer.Material = material;
                scene.AddSceneObject(triangle);
            }
            else
            {
                // create a simple cube
                SceneObject cube = SceneObject.CreateCube();
                cube.Renderer.Material = material;
                scene.AddSceneObject(cube);
            }

            Application.Instance.CurrentScene = scene;
            Application.Instance.Run();

            Application.Instance.Dispose();
        }
    }
}
