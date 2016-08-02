namespace Vapor
{
    using SharpDX;
    using SharpDX.Direct3D;
    using SharpDX.DXGI;
    using SharpDX.Direct3D11;

    public class Material : VaporObject
    {
        public VertexShader VertexShader { get; set; }

        public PixelShader PixelShader { get; set; }

        private InputLayout inputLayout;
        private InputElement[] inputElements = new InputElement[]
        {
            new InputElement("POSITION", 0, Format.R32G32B32_Float, 0)
        };

        public Material(string vertexShaderFilename, string pixelShaderFilename) : 
            this(new VertexShader(vertexShaderFilename), new PixelShader(pixelShaderFilename))
        {
        }

        public Material(VertexShader vertexShader, PixelShader pixelShader) : base("Material")
        {
            VertexShader = vertexShader;
            PixelShader = pixelShader;

            Application.Device.ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            inputLayout = new InputLayout(Application.Device, VertexShader.InputSignature, inputElements);

            Application.Device.ImmediateContext.InputAssembler.InputLayout = inputLayout;
        }

        public void Set()
        {
            if (VertexShader != null)
            {
                VertexShader.Set();
            }

            if (PixelShader != null)
            {
                PixelShader.Set();
            }
        }

        public void BindConstantBuffer<T>(ConstantBuffer constantBuffer, T bufferData, ShaderType shaderType = ShaderType.All) where T : struct
        {
            VertexShader.BindConstantBuffer(constantBuffer, bufferData);
        }

        public void SetConstantBuffer<T>(string name, T bufferData, ShaderType shaderType = ShaderType.All) where T : struct
        {
            if (shaderType == ShaderType.All)
            {
                VertexShader.SetConstantBuffer(name, bufferData);
                //PixelShader.SetConstantBuffer(name, bufferData);
            }
            else if(shaderType == ShaderType.Vertex)
            {
                VertexShader.SetConstantBuffer(name, bufferData);
            }
            else if (shaderType == ShaderType.Pixel)
            {
                //PixelShader.SetConstantBuffer(name, bufferData);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                VertexShader.Dispose();
                PixelShader.Dispose();
                inputLayout.Dispose();
            }
        }
    }
}
