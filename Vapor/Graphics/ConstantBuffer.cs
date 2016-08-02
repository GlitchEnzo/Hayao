namespace Vapor
{
    using SharpDX.D3DCompiler;
    using SharpDX.Direct3D11;

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// UPDATE: 
    /// Ignore most of the comment below.  You CAN get the slot # via ShaderReflection.GetResourceBindingDescription().BindPoint 
    /// 
    /// ORIGINAL:
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

        public ConstantBuffer(string variableName, int slot, int size) : base("ConstantBuffer")
        {
            Slot = slot;
            Size = size;
            VariableName = variableName;
            Buffer = new Buffer(Application.Device, Size, ResourceUsage.Default, BindFlags.ConstantBuffer, CpuAccessFlags.None, ResourceOptionFlags.None, 0);
        }

        public void SetData<T>(T bufferData) where T : struct
        {
            Application.Device.ImmediateContext.UpdateSubresource(ref bufferData, Buffer);
        }

        public static ConstantBuffer CreateFromStruct<T>(string variableName, int slot) where T : struct
        {
            int size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));

            ConstantBuffer buffer = new ConstantBuffer(variableName, slot, size);
            return buffer;
        }

        public static ConstantBuffer CreateFromStruct<T>(string variableName, int slot, T bufferData) where T : struct
        {
            int size = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));

            ConstantBuffer buffer = new ConstantBuffer(variableName, slot, size);
            buffer.SetData(bufferData);
            return buffer;
        }
    }
}
