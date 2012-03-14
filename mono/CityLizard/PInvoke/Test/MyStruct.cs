namespace CityLizard.PInvoke.Test
{
    using I = System.Runtime.InteropServices;

    public struct MyStruct
    {
        public int A;
        public int B;
        public byte C;
        // public bool D; // bool does not work if we return from function, even with the MarshalAs attribute.
    }

    public struct MyStructBool
    {
        public int A;
        public int B;
        public byte C;
        public bool D;
    }

    public struct MyBools
    {
        public bool A;
        public bool B;
        public bool C;
        public bool D;
    }

    public struct MyBools2
    {
        [I.MarshalAs(I.UnmanagedType.I1)]
        public bool A;
        [I.MarshalAs(I.UnmanagedType.I1)]
        public bool B;
        [I.MarshalAs(I.UnmanagedType.I1)]
        public bool C;
        [I.MarshalAs(I.UnmanagedType.I1)]
        public bool D;
    }

    public struct MyBools3
    {
        [I.MarshalAs(I.UnmanagedType.VariantBool)]
        public bool A;
        [I.MarshalAs(I.UnmanagedType.VariantBool)]
        public bool B;
        [I.MarshalAs(I.UnmanagedType.VariantBool)]
        public bool C;
        [I.MarshalAs(I.UnmanagedType.VariantBool)]
        public bool D;
    }

    public struct NoPack
    {
        public byte A;
        public long B;
    }

    [I.StructLayout(I.LayoutKind.Sequential, Pack = 1)]
    public struct Pack1
    {
        public byte A;
        public long B;
    }

    public class CX
    {
    }

    public struct Private
    {
        private int A;
        public int B;

        public Private(int a)
        {
            this.A = a;
            this.B = 0;
        }
    }

    public struct String
    {
        [I.MarshalAs(I.UnmanagedType.LPStr)]
        public string lpstr;
        [I.MarshalAs(I.UnmanagedType.LPWStr)]
        public string lpwstr;
        [I.MarshalAs(I.UnmanagedType.LPTStr)]
        public string lptstr;
    }

    /* // not supported.
    [I.StructLayout(I.LayoutKind.Explicit)]
    public struct EPack
    {
        [I.FieldOffset(0)]
        public byte A;
        [I.FieldOffset(1)]
        public long B;
    }
     * */

    /*
    public struct X
    {
        [I.MarshalAs(I.UnmanagedType.AnsiBStr)]
        public int a;
    }
     * */
}
