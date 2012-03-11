namespace CityLizard.PInvoke.Test
{
    using I = System.Runtime.InteropServices;

    [I.StructLayout(I.LayoutKind.Sequential)]
    public struct MyStruct
    {
        public int A;
        public int B;
        public byte C;
        // public bool D; // bool does not work, even with the MarshalAs attribute.
    }

    [I.StructLayout(I.LayoutKind.Sequential)]
    public struct MyStructBool
    {
        public int A;
        public int B;
        public byte C;
        public bool D;
    }
}
