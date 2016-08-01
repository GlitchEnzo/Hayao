namespace Vapor
{
    using SharpDX.D3DCompiler;
    using SharpDX.Direct3D11;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Interesting note about cbuffer slots.  If a cbuffer explictly lists it's register, ie:
    /// 
    /// cbuffer ConstantBuffer :  register(b1)
    /// {
    /// 	float4x4 constMatrix;
    /// }
    /// 
    /// Then the register number (b0, b1) becomes the slot number (0, 1).  EVEN IF ONE OF THEM IS COMPILED AWAY!
    /// 
    /// Meaning if you have b0 and b1 defined, but b0 is unused and therefore compiled away, b1 IS STILL SLOT 1!
    /// However, when you use ShaderReflection, only ONE cbuffer is seen, and it reports the cbuffer INDEX (which is 0), but not the SLOT (which is 1)!
    /// 
    /// It is impossible (I think) to look up the actual slot#/register# of a cbuffer, so it's better to NOT explictly 
    /// define registers so that the automatic registers and slots match each other.
    /// /// </remarks>
    public class ConstantBuffer : VaporObject
    {
        public int Slot { get; private set; }
        public Buffer Buffer { get; private set; }
        public string VariableName { get; private set; }
        public int Size { get; private set; }

        public ConstantBuffer(int slot, int size, string variableName) : base("ConstantBuffer")
        {
            Slot = slot;
            Size = size;
            VariableName = variableName;
            Buffer = new Buffer(Application.Device, Size, ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
        }

        public ConstantBuffer(ConstantBufferDescription desc, int slot) : base("ConstantBuffer")
        {
            Slot = slot;
            Size = desc.Size;
            VariableName = desc.Name;
            Buffer = new Buffer(Application.Device, Size, ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
        }

        public static ConstantBuffer CreateFromStruct<T>(string variableName, int slot = 0) where T : struct
        {
            // TODO: Allow default data to be set?
            int size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));

            ConstantBuffer buffer = new ConstantBuffer(slot, size, variableName);
            return buffer;
        }
    }
}
