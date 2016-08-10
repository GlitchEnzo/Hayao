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
            using (var pixelShaderByteCode = ShaderBytecode.CompileFromFile(filename, "main", "ps_4_0", ShaderFlags.Debug, EffectFlags.None, null, new ShaderIncludeHandler()))
            {
                if (pixelShaderByteCode.Bytecode == null)
                {
                    Log.Error("Shader Compilation Error: {0}", pixelShaderByteCode.Message);
                }
                else
                {
                    ShaderReflection reflection = new ShaderReflection(pixelShaderByteCode);
                    ReadShader(reflection);

                    pixelShader = new D3D11.PixelShader(Application.Device, pixelShaderByteCode);
                    //Set();
                }
            }
        }

        public void Set()
        {
            Application.Device.ImmediateContext.PixelShader.Set(pixelShader);

            // TODO: Does this produce garbage?
            foreach (var constantBuffer in constantBuffers.Values)
            {
                Application.Device.ImmediateContext.PixelShader.SetConstantBuffer(constantBuffer.Slot, constantBuffer.Buffer);
            }
        }

        /// <summary>
        /// Binds a NEW constant buffer to this shader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="constantBuffer"></param>
        /// <param name="bufferData"></param>
        public override void BindConstantBuffer<T>(ConstantBuffer constantBuffer, T bufferData)
        {
            Application.Device.ImmediateContext.PixelShader.SetConstantBuffer(constantBuffer.Slot, constantBuffer.Buffer);

            // store the buffer
            if (constantBuffers.ContainsKey(constantBuffer.VariableName))
            {
                constantBuffers[constantBuffer.VariableName].Dispose();
                constantBuffers[constantBuffer.VariableName] = constantBuffer;
            }
            else
            {
                constantBuffers.Add(constantBuffer.VariableName, constantBuffer);
            }

            Application.Device.ImmediateContext.UpdateSubresource(ref bufferData, constantBuffer.Buffer);
        }

        public override void SetTexture(string name, Texture2D texture)
        {
            //Application.Device.ImmediateContext.s(ref bufferData, constantBuffers[name].Buffer);
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
