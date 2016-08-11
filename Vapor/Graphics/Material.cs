namespace Vapor
{
    public class Material : VaporObject
    {
        public VertexShader VertexShader { get; set; }

        public PixelShader PixelShader { get; set; }

        public Material(string vertexShaderFilename, string pixelShaderFilename) : 
            this(new VertexShader(vertexShaderFilename), new PixelShader(pixelShaderFilename))
        {
        }

        public Material(VertexShader vertexShader, PixelShader pixelShader) : base("Material")
        {
            VertexShader = vertexShader;
            PixelShader = pixelShader;
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

        public void UpdateConstantBuffer<T>(string name, T bufferData, ShaderType shaderType = ShaderType.All) where T : struct
        {
            if (shaderType == ShaderType.All)
            {
                VertexShader.UpdateConstantBuffer(name, bufferData);
                //PixelShader.SetConstantBuffer(name, bufferData);
            }
            else if(shaderType == ShaderType.Vertex)
            {
                VertexShader.UpdateConstantBuffer(name, bufferData);
            }
            else if (shaderType == ShaderType.Pixel)
            {
                //PixelShader.SetConstantBuffer(name, bufferData);
            }
        }

        public void SetTexture(string name, Texture2D texture, ShaderType shaderType = ShaderType.All)
        {
            if (shaderType == ShaderType.All)
            {
                //VertexShader.UpdateConstantBuffer(name, bufferData);
                PixelShader.SetTexture(name, texture);
            }
            else if (shaderType == ShaderType.Vertex)
            {
                //VertexShader.UpdateConstantBuffer(name, bufferData);
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
            }
        }
    }
}
