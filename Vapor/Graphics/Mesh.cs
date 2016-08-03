namespace Vapor
{
    using SharpDX;
    using D3D11 = SharpDX.Direct3D11;

    public class Mesh : VaporObject
    {
        private Vector3[] vertices;

        public Vector3[] Vertices
        {
            get
            {
                return vertices;
            }
            set
            {
                vertices = value;
                VertexCount = vertices.Length / 3;

                if (vertexBuffer != null)
                {
                    vertexBuffer.Dispose();
                }

                vertexBuffer = D3D11.Buffer.Create(Application.Device, D3D11.BindFlags.VertexBuffer, vertices);
                vertexBufferBinding = new D3D11.VertexBufferBinding(vertexBuffer, Utilities.SizeOf<Vector3>(), 0);
            }
        }

        public int VertexCount { get; private set; }

        private D3D11.Buffer vertexBuffer;
        private D3D11.VertexBufferBinding vertexBufferBinding;

        public Mesh() : base("Mesh")
        {
            
        }

        public Mesh(string name) : base("Mesh: " + name)
        {

        }

        public void Draw(Material material)
        {
            // Set vertex buffer
            Application.Device.ImmediateContext.InputAssembler.SetVertexBuffers(0, vertexBufferBinding);

            // Draw the mesh
            Application.Device.ImmediateContext.Draw(vertices.Length, 0);
        }

        public static Mesh CreateTriangle()
        {
            //  0-----1
            //   \   /
            //     2
            Mesh mesh = new Mesh("Triangle");
            mesh.Vertices = new Vector3[] { new Vector3(-0.5f, 0.5f, 0.0f), new Vector3(0.5f, 0.5f, 0.0f), new Vector3(0.0f, -0.5f, 0.0f) };

            return mesh;
        }

        public static Mesh CreateCube()
        {
            // 0------1
            // |      |\
            // |      | \
            // 3------2  ?
            //  \      \ |
            //   \      \|
            //    ?------?
            Mesh mesh = new Mesh("Cube");
            mesh.Vertices = new Vector3[] {
                new Vector3(-1.0f, 1.0f, -1.0f),  // Top    Left    Back    0
                new Vector3(1.0f, 1.0f, -1.0f),   // Top    Right   Back    1
                new Vector3(1.0f, 1.0f, 1.0f),    // Top    Right   Front   2
                new Vector3(-1.0f, 1.0f, 1.0f),   // Top    Left    Front   3
                new Vector3(-1.0f, -1.0f, -1.0f), // Bottom Left    Back    4
                new Vector3(1.0f, -1.0f, -1.0f),  // Bottom Right   Back    5
                new Vector3(1.0f, -1.0f, 1.0f),   // Bottom Right   Front   6
                new Vector3(-1.0f, -1.0f, 1.0f)   // Bottom Left    Front   7 
            };

            return mesh;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                vertexBuffer.Dispose();
            }
        }
    }
}
