namespace Vapor
{
    using System;
    using System.IO;
    using SharpDX.D3DCompiler;

    class ShaderIncludeHandler : Include
    {
        static string includeDirectory = ".\\Shaders\\";

        public Stream Open(IncludeType type, string fileName, Stream parentStream)
        {
            Log.Info("Including {0}", fileName);

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
                return null;
            }

            set
            {
                Log.Info("Not Implemented!");
            }
        }

        public void Dispose()
        {
            if (Shadow != null)
            {
                Shadow.Dispose();
            }
        }
    }
}
