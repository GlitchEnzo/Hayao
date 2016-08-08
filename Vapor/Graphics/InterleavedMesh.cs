namespace Vapor
{
    using SharpDX;
    using SharpDX.Direct3D;
    using SharpDX.Direct3D11;
    using SharpDX.DXGI;

    public class InterleavedMesh<T> : VaporObject where T : struct, IVertexType
    {
        private T[] vertexData;
        public T[] VertexData
        {
            get
            {
                return vertexData;
            }
            set
            {
                vertexData = value;

                if (vertexBuffer != null)
                {
                    vertexBuffer.Dispose();
                }

                vertexBuffer = Buffer.Create(Application.Device, BindFlags.VertexBuffer, vertexData);
                vertexBufferBinding = new VertexBufferBinding(vertexBuffer, Utilities.SizeOf<T>(), 0);
            }
        }
        
        private Buffer vertexBuffer;
        private VertexBufferBinding vertexBufferBinding;

        private uint[] indices;
        private Buffer indexBuffer;

        private InputLayout inputLayout;
        private InputElement[] inputElements;

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

                indexBuffer = Buffer.Create(Application.Device, BindFlags.IndexBuffer, indices);
            }
        }

        public InterleavedMesh() : this("InterleavedMesh")
        {

        }

        public InterleavedMesh(string name) : base(name)
        {
            Application.Device.ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            inputElements = new T().GetInputElements();
        }

        public void Draw(Material material)
        {
            // set the data input layout
            inputLayout = new InputLayout(Application.Device, material.VertexShader.InputSignature, inputElements);

            Application.Device.ImmediateContext.InputAssembler.InputLayout = inputLayout;

            // Set vertex buffer
            Application.Device.ImmediateContext.InputAssembler.SetVertexBuffers(0, vertexBufferBinding);

            // Set the index buffer
            Application.Device.ImmediateContext.InputAssembler.SetIndexBuffer(indexBuffer, Format.R32_UInt, 0);

            // Draw the mesh
            Application.Device.ImmediateContext.DrawIndexed(Indices.Length, 0, 0);
        }

        public static InterleavedMesh<VertexPosition> CreateTriangle()
        {
            //  0-----1
            //   \   /
            //     2
            InterleavedMesh<VertexPosition> mesh = new InterleavedMesh<VertexPosition>("Triangle");
            mesh.VertexData = new VertexPosition[] 
            {
                new VertexPosition(new Vector3(-0.5f, 0.5f, 0.0f)),
                new VertexPosition(new Vector3(0.5f, 0.5f, 0.0f)),
                new VertexPosition(new Vector3(0.0f, -0.5f, 0.0f))
            };
            mesh.Indices = new uint[] { 0, 1, 2 };

            return mesh;
        }
    }
}
