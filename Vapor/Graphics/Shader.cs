namespace Vapor
{
    using System.Collections.Generic;

    public abstract class Shader : VaporObject
    {
        protected Dictionary<string, ConstantBuffer> constantBuffers = new Dictionary<string, ConstantBuffer>();

        public Shader(string name) : base(name)
        {
        }

        public abstract void SetConstantBuffer<T>(string name, T bufferData) where T : struct;
    }
}
