﻿namespace CityLizard.PInvoke.Test
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

    [I.StructLayout(I.LayoutKind.Sequential, CharSet = I.CharSet.Auto)]
    public struct Chars
    {
        public char A;
        public char B;
        public char C;
    }

    public struct AnsiChars
    {
        public char A;
        public char B;
        public char C;
    }

    public struct BStr
    {
        [I.MarshalAs(I.UnmanagedType.BStr)]
        public string A;
    }

    [I.StructLayout(I.LayoutKind.Sequential, CharSet = I.CharSet.Unicode)]
    public struct String
    {
        // LPSTR or LPWSTR
        public string def;
        [I.MarshalAs(I.UnmanagedType.LPStr)]
        public string lpstr;
        [I.MarshalAs(I.UnmanagedType.LPWStr)]
        public string lpwstr;
        [I.MarshalAs(I.UnmanagedType.LPTStr)]
        public string lptstr;
        [I.MarshalAs(I.UnmanagedType.ByValTStr, SizeConst = 10)]
        public string x;
    }

    // by default it is ANSI.
    [I.StructLayout(I.LayoutKind.Sequential /*, CharSet = I.CharSet.Ansi*/)]
    public struct StringAnsi
    {
        // LPSTR or LPWSTR
        public string def;
        [I.MarshalAs(I.UnmanagedType.LPStr)]
        public string lpstr;
        [I.MarshalAs(I.UnmanagedType.LPWStr)]
        public string lpwstr;
        [I.MarshalAs(I.UnmanagedType.LPTStr)]
        public string lptstr;
        [I.MarshalAs(I.UnmanagedType.ByValTStr, SizeConst = 10)]
        public string x;
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

    public struct ByValArray
    {
        // [I.MarshalAs(I.UnmanagedType.ByValArray, ArraySubType=I.UnmanagedType.ByValArray)]
        [I.MarshalAs(I.UnmanagedType.ByValArray, ArraySubType = I.UnmanagedType.I2, SizeConst = 100)]
        public byte[] X;
    }
}
