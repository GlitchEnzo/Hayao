namespace Vapor
{
    using SharpDX.D3DCompiler;
    using System.Collections.Generic;

    public abstract class Shader : VaporObject
    {
        protected Dictionary<string, ConstantBuffer> constantBuffers = new Dictionary<string, ConstantBuffer>();

        protected Dictionary<string, Sampler> samplers = new Dictionary<string, Sampler>();

        public Shader(string name) : base(name)
        {
        }

        /// <summary>
        /// Read the shader and automatically get buffers, textures, sampler, etc from it.
        /// </summary>
        /// <param name="reflection">The <see cref="ShaderReflection"/> object formed from the shader bytecode.</param>
        protected void ReadShader(ShaderReflection reflection)
        {
            for (int i = 0; i < reflection.Description.BoundResources; i++)
            {
                var resourceInfo = reflection.GetResourceBindingDescription(i);
                switch (resourceInfo.Type)
                {
                    case ShaderInputType.ConstantBuffer:
                        var constantBufferInfo = reflection.GetConstantBuffer(resourceInfo.Name);

                        ConstantBuffer constantBuffer = new ConstantBuffer(resourceInfo.Name, resourceInfo.BindPoint, constantBufferInfo.Description.Size);
                        constantBuffers.Add(constantBuffer.VariableName, constantBuffer);

                        //Application.Device.ImmediateContext.PixelShader.SetConstantBuffer(constantBuffer.Slot, constantBuffer.Buffer);
                        break;
                    case ShaderInputType.Texture:
                        break;
                    case ShaderInputType.Sampler:
                        Sampler sampler = new Sampler(resourceInfo.Name, resourceInfo.BindPoint);
                        samplers.Add(sampler.VariableName, sampler);

                        //Application.Device.ImmediateContext.PixelShader.SetSampler(resourceInfo.BindPoint, sampler.SamplerState);
                        break;
                }
            }
        }

        public abstract void BindConstantBuffer<T>(ConstantBuffer constantBuffer, T bufferData) where T : struct;

        public ConstantBuffer GetConstantBuffer(string name)
        {
            if (constantBuffers.ContainsKey(name))
            {
                return constantBuffers[name];
            }

            Log.Error("{0}: No constant buffer with that name: {1}", Name, name);
            return null;
        }
        
        /// <summary>
        /// Updates an EXISTING constant buffer with the given data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="bufferData"></param>
        public void SetConstantBuffer<T>(string name, T bufferData) where T : struct
        {
            if (constantBuffers.ContainsKey(name))
            {
                Application.Device.ImmediateContext.UpdateSubresource(ref bufferData, constantBuffers[name].Buffer);
            }
            else if (name != "VaporConstants")
            {
                Log.Error("{0}: No constant buffer with that name: {1}", Name, name);
            }
        }

        public Sampler GetSampler(string name)
        {
            if (samplers.ContainsKey(name))
            {
                return samplers[name];
            }

            Log.Error("{0}: No sampler with that name: {1}", Name, name);
            return null;
        }

        public abstract void SetTexture(string name, Texture2D texture);
    }
}
