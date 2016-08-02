namespace Vapor
{
    using SharpDX.D3DCompiler;
    using D3D11 = SharpDX.Direct3D11;

    public class PixelShader : Shader
    {
        private D3D11.PixelShader pixelShader;

        public PixelShader(string filename) : base("PixelShader")
        {
            // Compile the pixel shader code
            using (var pixelShaderByteCode = ShaderBytecode.CompileFromFile(filename, "main", "ps_4_0", ShaderFlags.Debug))
            {
                pixelShader = new D3D11.PixelShader(Application.Device, pixelShaderByteCode);
            }
        }

        public void Set()
        {
            Application.Device.ImmediateContext.PixelShader.Set(pixelShader);
        }

        public override void SetConstantBuffer<T>(string name, T bufferData)
        {
            Application.Device.ImmediateContext.UpdateSubresource(ref bufferData, constantBuffers[name].Buffer);
        }

        public override ConstantBuffer GetConstantBuffer(string name)
        {
            if (constantBuffers.ContainsKey(name))
            {
                return constantBuffers[name];
            }

            Log.Error("PS: No constant buffer with that name: {0}", name);
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                pixelShader.Dispose();
            }
        }
    }
}
