namespace Vapor
{
    using SharpDX.D3DCompiler;
    using D3D11 = SharpDX.Direct3D11;

    public class PixelShader : VaporObject
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                pixelShader.Dispose();
            }
        }
    }
}
