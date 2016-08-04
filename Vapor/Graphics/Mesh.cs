namespace Vapor
{
    using SharpDX;
    using D3D11 = SharpDX.Direct3D11;

    public class Mesh : VaporObject
    {
        private Vector3[] vertices;
        public int VertexCount { get; private set; }
        private D3D11.Buffer vertexBuffer;
        private D3D11.VertexBufferBinding vertexBufferBinding;

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

        private uint[] indices;
        private D3D11.Buffer indexBuffer;

        public uint[] Indices
        {
            get
            {
                return indices;
            }
            set
            {
                indices = value;

                if (indexBuffer != null)
                {
                    indexBuffer.Dispose();
                }

                indexBuffer = D3D11.Buffer.Create(Application.Device, D3D11.BindFlags.IndexBuffer, indices);
            }
        }

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

            // Set the index buffer
            Application.Device.ImmediateContext.InputAssembler.SetIndexBuffer(indexBuffer, SharpDX.DXGI.Format.R32_UInt, 0);

            // Draw the mesh
            //Application.Device.ImmediateContext.Draw(vertices.Length, 0);
            Application.Device.ImmediateContext.DrawIndexed(Indices.Length, 0, 0);
        }

        public static Mesh CreateTriangle()
        {
            //  0-----1
            //   \   /
            //     2
            Mesh mesh = new Mesh("Triangle");
            mesh.Vertices = new Vector3[] { new Vector3(-0.5f, 0.5f, 0.0f), new Vector3(0.5f, 0.5f, 0.0f), new Vector3(0.0f, -0.5f, 0.0f) };
            mesh.Indices = new uint[] { 0, 1, 2 };

            return mesh;
        }

        public static Mesh CreateCube()
        {
            // 0------1
            // |      |\
            // |      | \
            // 3------2  5
            //  \      \ |
            //   \      \|
            //    7------6

            // 5------4
            // |      |\
            // |      | \
            // 6------7  0
            //  \      \ |
            //   \      \|
            //    2------3
            Mesh mesh = new Mesh("Cube");
            mesh.Vertices = new Vector3[] {
                new Vector3(-1.0f,  1.0f, -1.0f), // Top    Left    Back    0
                new Vector3( 1.0f,  1.0f, -1.0f), // Top    Right   Back    1
                new Vector3( 1.0f,  1.0f,  1.0f), // Top    Right   Front   2
                new Vector3(-1.0f,  1.0f,  1.0f), // Top    Left    Front   3
                new Vector3(-1.0f, -1.0f, -1.0f), // Bottom Left    Back    4
                new Vector3( 1.0f, -1.0f, -1.0f), // Bottom Right   Back    5
                new Vector3( 1.0f, -1.0f,  1.0f), // Bottom Right   Front   6
                new Vector3(-1.0f, -1.0f,  1.0f)  // Bottom Left    Front   7 
            };

            mesh.Indices = new uint[] {
                0, 1, 2, // Top
                0, 2, 3,
                0, 3, 4, // Left
                3, 7, 4,
                1, 5, 2, // Right
                2, 5, 6,
                2, 6, 3, // Front
                3, 6, 7,
                0, 4, 1, // Back
                1, 4, 5,
                4, 7, 6, // Bottom
                4, 6, 5
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
