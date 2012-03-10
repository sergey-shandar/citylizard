namespace CityLizard.PInvoke.Call
{
    using S = System;
    using I = System.Runtime.InteropServices;

    class Program
    {
        static void Main(string[] args)
        {
            // Unable to load DLL 'citylizard_pinvoke_test_cpp.dll': The 
            // specified module could not be found. (Exception from HRESULT:
            // 0x8007007E)
            //
            // Unable to find an entry point named 'A' in DLL 
            // 'citylizard_pinvoke_test_cpp.dll'.
            //
            // An attempt was made to load a program with an incorrect format. 
            // (Exception from HRESULT: 0x8007000B)
            //
            // A call to PInvoke function 
            // 'CityLizard.PInvoke.Test!CityLizard.PInvoke.Test.MyClass::A' has 
            // unbalanced the stack. This is likely because the managed PInvoke 
            // signature does not match the unmanaged target signature. Check 
            // that the calling convention and parameters of the PInvoke 
            // signature match the target unmanaged signature.

            // Test.MyClass.A(new Test.MyStruct());

            // int x = Test.MyClass.C();
            int a = Test.MyClass.A();
            int b = Test.MyClass.B();

            try
            {
                Test.MyClass.C();
                throw new S.Exception("the Test.MyClass.C() function must fail.");
            }
            catch (I.COMException e)
            {
                if (e.ErrorCode != -0x7FFFBFFB)
                {
                    throw new S.Exception("the Test.MyClass.C() throws an unknown COM exception.");
                }
            }

            int d = Test.MyClass.D(127, 12);
            var ee = Test.MyClass.E(Test.MyEnum.C, Test.MyEnum.B);

            Test.MyClass.CheckBool(true, true, true, true, 0x1111);
            Test.MyClass.CheckBool(true, false, true, true, 0x1011);
            Test.MyClass.CheckBool(true, false, false, true, 0x1001);
            Test.MyClass.CheckBool(false, false, false, true, 0x0001);
            Test.MyClass.CheckBool(false, false, false, false, 0x0000);
        }
    }
}
