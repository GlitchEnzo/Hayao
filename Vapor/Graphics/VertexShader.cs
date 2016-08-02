namespace Vapor
{
    using System.Collections.Generic;
    using SharpDX;
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

                    

                    vertexShader = new D3D11.VertexShader(Application.Device, vertexShaderByteCode);
                    //Set();

                    // automatically create constant buffers from the shader description
                    for (int i = 0; i < reflection.Description.ConstantBuffers; i++)
                    {
                        var constantBufferInfo = reflection.GetConstantBuffer(i);
                        var resourceInfo = reflection.GetResourceBindingDescription(constantBufferInfo.Description.Name);
                        ConstantBuffer constantBuffer = new ConstantBuffer(constantBufferInfo.Description, resourceInfo.BindPoint);
                        constantBuffers.Add(constantBuffer.VariableName, constantBuffer);
                        Application.Device.ImmediateContext.VertexShader.SetConstantBuffer(constantBuffer.Slot, constantBuffer.Buffer);
                    }

                    // Read input signature from shader code
                    InputSignature = ShaderSignature.GetInputSignature(vertexShaderByteCode);
                }
            }
        }

        public void Set()
        {
            Application.Device.ImmediateContext.VertexShader.Set(vertexShader);
        }

        public void SetConstantBuffer<T>(ConstantBuffer constantBuffer, T bufferData) where T : struct
        {
            // TODO: Only create the buffer once and store it
            Application.Device.ImmediateContext.VertexShader.SetConstantBuffer(constantBuffer.Slot, constantBuffer.Buffer);
            Application.Device.ImmediateContext.UpdateSubresource(ref bufferData, constantBuffer.Buffer);

            //Application.Device.ImmediateContext.UpdateSubresource(ref bufferData, constantBuffers[constantBuffer.VariableName].Buffer);
        }

        public override void SetConstantBuffer<T>(string name, T bufferData)
        {
            if (constantBuffers.ContainsKey(name))
            {
                Application.Device.ImmediateContext.UpdateSubresource(ref bufferData, constantBuffers[name].Buffer);
            }
            else
            {
                Log.Error("VS: No constant buffer with that name: {0}", name);
            }
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
