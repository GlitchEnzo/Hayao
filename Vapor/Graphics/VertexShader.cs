namespace Vapor
{
    using SharpDX.D3DCompiler;
    using D3D11 = SharpDX.Direct3D11;

    public class VertexShader : Shader
    {
        private D3D11.VertexShader vertexShader;
        public ShaderSignature InputSignature { get; private set; }
        private ShaderReflection reflection;

        public VertexShader(string filename) : base("VertexShader - " + filename)
        {
            // Compile the vertex shader code
            using (var vertexShaderByteCode = ShaderBytecode.CompileFromFile(filename, "main", "vs_4_0", ShaderFlags.Debug, EffectFlags.None, null, new ShaderIncludeHandler()))
            {
                if (vertexShaderByteCode.Bytecode == null)
                {
                    Log.Error("Shader Compilation Error: {0}", vertexShaderByteCode.Message);
                }
                else
                {
                    reflection = new ShaderReflection(vertexShaderByteCode);
                    ReadShader(reflection);

                    vertexShader = new D3D11.VertexShader(Application.Device, vertexShaderByteCode);

                    // Read input signature from shader code
                    InputSignature = ShaderSignature.GetInputSignature(vertexShaderByteCode);
                }
            }
        }

        public override void Set()
        {
            Application.Device.ImmediateContext.VertexShader.Set(vertexShader);

            Set(Application.Device.ImmediateContext.VertexShader);
        }

        /// <summary>
        /// Binds a NEW constant buffer to this shader.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="constantBuffer"></param>
        /// <param name="bufferData"></param>
        public override void BindConstantBuffer<T>(ConstantBuffer constantBuffer, T bufferData)
        {
            Application.Device.ImmediateContext.VertexShader.SetConstantBuffer(constantBuffer.Slot, constantBuffer.Buffer);

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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                InputSignature.Dispose();
                vertexShader.Dispose();
            }
        }
    }
}
