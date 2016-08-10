namespace Vapor
{
    using SharpDX.Direct3D11;

    public class Sampler : VaporObject
    {
        public int Slot { get; private set; }
        public SamplerState SamplerState { get; private set; }
        public string VariableName { get; private set; }
        //public int Size { get; private set; }

        public Sampler(string variableName, int slot) : base("Sampler")
        {
            VariableName = variableName;
            Slot = slot;

            SamplerStateDescription desc = new SamplerStateDescription();
            desc.AddressU = TextureAddressMode.Wrap;
            desc.AddressV = TextureAddressMode.Wrap;
            desc.AddressW = TextureAddressMode.Wrap;

            SamplerState = new SamplerState(Application.Device, desc);
        }
    }
}
