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
}
