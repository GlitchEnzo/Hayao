namespace Vapor
{
    using SharpDX;
    using SharpDX.DXGI;
    using SharpDX.WIC;
    using D3D11 = SharpDX.Direct3D11;

    public class Texture2D : VaporObject
    {
        public int Slot { get; set; }

        public D3D11.ShaderResourceView ShaderResourceView { get; private set; }

        public D3D11.Texture2D Texture { get; private set; }

        private static readonly ImagingFactory imagingFactory = new ImagingFactory();

        public Texture2D() : base("Texture2D")
        {
        }

        private static BitmapSource LoadBitmap(string filename)
        {
            var decoder = new BitmapDecoder(imagingFactory, filename, DecodeOptions.CacheOnDemand);

            var frame = decoder.GetFrame(0);

            var converter = new FormatConverter(imagingFactory);

            converter.Initialize(frame, PixelFormat.Format32bppPRGBA, BitmapDitherType.None, null, 0.0, BitmapPaletteType.Custom);
            return converter;
        }

        public static D3D11.Texture2D FromBitmap(BitmapSource bsource)
        {
            D3D11.Texture2DDescription desc;
            desc.Width = bsource.Size.Width;
            desc.Height = bsource.Size.Height;
            desc.ArraySize = 1;
            desc.BindFlags = D3D11.BindFlags.ShaderResource;
            desc.Usage = D3D11.ResourceUsage.Default;
            desc.CpuAccessFlags = D3D11.CpuAccessFlags.None;
            desc.Format = Format.R8G8B8A8_UNorm;
            desc.MipLevels = 1;
            desc.OptionFlags = D3D11.ResourceOptionFlags.None;
            desc.SampleDescription.Count = 1;
            desc.SampleDescription.Quality = 0;

            var s = new DataStream(bsource.Size.Height * bsource.Size.Width * 4, true, true);
            bsource.CopyPixels(bsource.Size.Width * 4, s);

            var rect = new DataRectangle(s.DataPointer, bsource.Size.Width * 4);

            var t2D = new D3D11.Texture2D(Application.Device, desc, rect);

            return t2D;
        }

        public static Texture2D FromFile(string filename)
        {
            Texture2D texture = new Texture2D();
            var bSource = LoadBitmap(filename);
            texture.Texture = FromBitmap(bSource);
            texture.ShaderResourceView = new D3D11.ShaderResourceView(Application.Device, texture.Texture);
            return texture;
        }
    }
}
