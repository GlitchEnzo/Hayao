namespace Hayao
{
    using SharpDX; // needed for math (Vector, Matrix, Color, etc)
    using System.Runtime.InteropServices;
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

        [StructLayout(LayoutKind.Sequential)]
        struct TestBuffer
        {
            public Matrix matrix;
        }

        static void UseVapor()
        {
            Application.Instance.WindowTitle = "Hayao";

            //ConstantBuffer constantBuffer = ConstantBuffer.CreateFromStruct<TestBuffer>("TestBuffer", 1);
            //TestBuffer testBuffer = new TestBuffer();
            //testBuffer.matrix = Matrix.Identity;

            // create the scene
            Scene scene = new Scene("MyScene");

            // create a simple camera
            SceneObject cameraObject = SceneObject.CreateCamera();
            //cameraObject.Transform.Position = new Vector3(0.0f, 0.0f, -7.0f);
            Camera camera = cameraObject.GetComponent<Camera>();
            camera.BackgroundColor = new Color(32, 103, 178);
            scene.AddSceneObject(cameraObject);

            // create a simple triangle
            Material material = new Material("Shaders//vertexShader.hlsl", "Shaders//pixelShader.hlsl");
            //material.SetConstantBuffer(constantBuffer, testBuffer);
            //material.SetConstantBuffer("TestBuffer", testBuffer);
            SceneObject triangle = SceneObject.CreateTriangle();
            triangle.Renderer.Material = material;
            scene.AddSceneObject(triangle);

            Application.Instance.CurrentScene = scene;
            Application.Instance.Run();

            Application.Instance.Dispose();
        }
    }
}
