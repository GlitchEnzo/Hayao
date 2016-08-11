namespace Vapor
{
    using SharpDX.D3DCompiler;
    using SharpDX.Direct3D11;
    using System.Collections.Generic;

    public abstract class Shader : VaporObject
    {
        protected Dictionary<string, ConstantBuffer> constantBuffers = new Dictionary<string, ConstantBuffer>();

        protected Dictionary<string, Sampler> samplers = new Dictionary<string, Sampler>();

        protected Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        public Shader(string name) : base(name)
        {
        }

        public abstract void Set();

        protected void Set(CommonShaderStage shaderStage)
        {
            // TODO: Does this produce garbage?
            foreach (var constantBuffer in constantBuffers.Values)
            {
                shaderStage.SetConstantBuffer(constantBuffer.Slot, constantBuffer.Buffer);
            }

            foreach (var sampler in samplers.Values)
            {
                shaderStage.SetSampler(sampler.Slot, sampler.SamplerState);
            }

            foreach (var texture in textures.Values)
            {
                shaderStage.SetShaderResource(texture.Slot, texture.ShaderResourceView);
            }
        }

        /// <summary>
        /// Read the shader and automatically get buffers, textures, samplers, etc from it.
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
                        break;
                    case ShaderInputType.Texture:
                        Texture2D texture = new Texture2D();
                        texture.Slot = resourceInfo.BindPoint;
                        textures.Add(resourceInfo.Name, texture);
                        break;
                    case ShaderInputType.Sampler:
                        Sampler sampler = new Sampler(resourceInfo.Name, resourceInfo.BindPoint);
                        samplers.Add(sampler.VariableName, sampler);
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
        public void UpdateConstantBuffer<T>(string name, T bufferData) where T : struct
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

        public void UpdateSampler(string name, Sampler sampler)
        {
            if (samplers.ContainsKey(name))
            {
                samplers[name] = sampler;

                // TODO: Set the sampler or just let the Set() method do it next frame?
            }
            else
            {
                Log.Error("{0}: No sampler with that name: {1}", Name, name);
            }
        }

        public void SetTexture(string name, Texture2D texture)
        {
            //Application.Device.ImmediateContext.MapSubresource()  <-- For dynamic textures/buffers (changing every frame)
            //Application.Device.ImmediateContext.UpdateSubresource(texture.ShaderResourceView., )

            if (textures.ContainsKey(name))
            {
                texture.Slot = textures[name].Slot;
                textures[name] = texture;

                // TODO: Set the sampler or just let the Set() method do it next frame?
            }
            else
            {
                Log.Error("{0}: No texture with that name: {1}", Name, name);
            }
        }
    }
}
