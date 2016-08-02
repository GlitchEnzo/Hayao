namespace Vapor
{
    using System;
    using System.IO;
    using SharpDX.D3DCompiler;

    class ShaderIncludeHandler : Include
    {
        static string includeDirectory = ".\\Shaders\\";

        private IDisposable shadow;

        public Stream Open(IncludeType type, string fileName, Stream parentStream)
        {
            Log.Info("Including {0}", includeDirectory + fileName);

            Stream stream = new FileStream(includeDirectory + fileName, FileMode.Open);

            return stream;
        }

        public void Close(Stream stream)
        {
            stream.Close();
            stream.Dispose();
        }

        public IDisposable Shadow
        {
            get
            {
                //Log.Info("ShaderIncludeHandler.Shadow.getter() - Not Implemented!");
                return null;
            }

            set
            {
                //Log.Info("ShaderIncludeHandler.Shadow.setter() - {0}", value);
                shadow = value;
            }
        }

        public void Dispose()
        {
            if (Shadow != null)
            {
                //Log.Info("ShaderIncludeHandler.Dispose - Disposing Shadow");
                Shadow.Dispose();
            }
        }
    }
}
