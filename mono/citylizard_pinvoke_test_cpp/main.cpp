#include <interface.hpp>

#include <iostream>
#include <windows.hpp>

static_assert(::CityLizard::PInvoke::Test::MyBigEnum::MaxValue == LLONG_MAX, "xxx");
static_assert(::CityLizard::PInvoke::Test::MyBigEnum::MinValue == LLONG_MIN, "xxx");
static_assert(::CityLizard::PInvoke::Test::MyPBigEnum::MaxValue == ULLONG_MAX, "xxx");

::LONG __stdcall A() { return 0xA; }

::HRESULT __cdecl B(::LONG *p) { *p = 0xB; return S_OK; }

::HRESULT WINAPI C() { return E_FAIL; }

::LONG WINAPI D(::BYTE a, ::SHORT b) { return a + b; }

::CityLizard::PInvoke::Test::MyEnum::value_type WINAPI E(
	::CityLizard::PInvoke::Test::MyEnum::value_type a,
	::CityLizard::PInvoke::Test::MyEnum::value_type b)
{ 
	return a - b;
}

::CityLizard::PInvoke::Test::MyBigEnum::value_type __cdecl BigE(
	::CityLizard::PInvoke::Test::MyBigEnum::value_type a,
	::CityLizard::PInvoke::Test::MyBigEnum::value_type b)
{ 
	return a - b;
}

::HRESULT WINAPI PBigE(
	::CityLizard::PInvoke::Test::MyPBigEnum::value_type a, 
	::CityLizard::PInvoke::Test::MyPBigEnum::value_type b, 
	::CityLizard::PInvoke::Test::MyPBigEnum::value_type *p)
{
	*p = a - b;
	return S_OK;
}

::CityLizard::PInvoke::Test::IntEnum::value_type __stdcall IntE(
    ::CityLizard::PInvoke::Test::UIntEnum::value_type a, 
    ::CityLizard::PInvoke::Test::UIntEnum::value_type b)
{
    return a - b;
}

static_assert(sizeof(BOOL) == 4, "BOOL");
static_assert(TRUE == 1, "TRUE");
static_assert(FALSE == 0, "FALSE");

static_assert(sizeof(BOOLEAN) == 1, "BOOLEAN");

static_assert(sizeof(VARIANT_BOOL) == 2, "VARIANT_BOOL");
static_assert(VARIANT_TRUE == -1, "VARIANT_TRUE");
static_assert(VARIANT_FALSE == 0, "VARIANT_FALSE");

static_assert(sizeof(bool) == 1, "bool");
static_assert(true == 1, "true");
static_assert(false == 0, "false");

::HRESULT WINAPI CheckBool(BOOL a, BOOL b, BOOL c, BOOL d, ::LONG x)
{
    return 
        (((x >> 12) & 1) == a) && 
        (((x >>  8) & 1) == b) &&
        (((x >>  4) & 1) == c) &&
        (((x >>  0) & 1) == d) ?
        S_OK:
        E_FAIL;
}

::HRESULT WINAPI CheckBool2(CHAR a, CHAR b, CHAR c, CHAR d, ::LONG x)
{
    return 
        (((x >> 12) & 1) == a) && 
        (((x >>  8) & 1) == b) &&
        (((x >>  4) & 1) == c) &&
        (((x >>  0) & 1) == d) ?
        S_OK:
        E_FAIL;
}

::HRESULT WINAPI CheckBool3(::VARIANT_BOOL a, ::VARIANT_BOOL b, ::VARIANT_BOOL c, ::VARIANT_BOOL d, ::LONG x)
{
    return 
        (((x >> 12) & 1) == (a == VARIANT_TRUE)) && 
        (((x >>  8) & 1) == (b == VARIANT_TRUE)) &&
        (((x >>  4) & 1) == (c == VARIANT_TRUE)) &&
        (((x >>  0) & 1) == (d == VARIANT_TRUE)) ?
        S_OK:
        E_FAIL;
}

::BOOL WINAPI RetBool()
{
    return TRUE;
}

::VARIANT_BOOL WINAPI RetBool3()
{
    return VARIANT_TRUE;
}


::HRESULT WINAPI RetStructP(::CityLizard::PInvoke::Test::MyStruct *p)
{
    p->A = 0x0A;
    p->B = 0x0B;
    return S_OK;
}

::CityLizard::PInvoke::Test::MyStruct WINAPI RetStruct()
{
    ::CityLizard::PInvoke::Test::MyStruct a;
    a.A = 0x0A;
    a.B = 0x0B;
    return a;
}

::HRESULT WINAPI SetStruct(::CityLizard::PInvoke::Test::MyStruct s)
{
    return S_OK;
}

::HRESULT WINAPI SetStructBool(::CityLizard::PInvoke::Test::MyStructBool)
{
    return S_OK;
}

::HRESULT WINAPI CheckBools(::CityLizard::PInvoke::Test::MyBools y, ::LONG x)
{
    return ::CheckBool(y.A, y.B, y.C, y.D, x);
}

::HRESULT WINAPI CheckBools2(::CityLizard::PInvoke::Test::MyBools2 y, ::LONG x)
{
    return ::CheckBool2(y.A, y.B, y.C, y.D, x);
}

::HRESULT WINAPI CheckBools3(::CityLizard::PInvoke::Test::MyBools3 y, ::LONG x)
{
    return ::CheckBool3(y.A, y.B, y.C, y.D, x);
}

void WINAPI RetBoolOut(::CityLizard::PInvoke::Test::MyBools3 *p)
{
    p->A = true;
    p->B = false;
    p->C = true;
    p->D = true;
}

::HRESULT WINAPI PackTest(::CityLizard::PInvoke::Test::NoPack n, ::CityLizard::PInvoke::Test::Pack1 p1)
{
    return 
        n.A == 0x12 && n.B == 0x3456789A &&
        p1.A == 0x12 && p1.B == 0x3456789A ? S_OK: E_FAIL;
}

::HRESULT WINAPI PrivateStruct(::CityLizard::PInvoke::Test::Private s)
{
    return s.B == 0 ? S_OK: E_FAIL;
}

::HRESULT WINAPI StringStructAnsi(::CityLizard::PInvoke::Test::StringAnsi x)
{
    return
        (::strcmp(x.def, "def") == 0) &&
        (::strcmp(x.lpstr, "lpstr") == 0) &&
        (::wcscmp(x.lpwstr, L"lpwstr") == 0) &&
        (::wcscmp(x.lptstr, L"lptstr") == 0) &&
        (::strcmp(x.x, "::TCHAR[1") == 0) ?
        S_OK :
        E_FAIL;
}

::HRESULT WINAPI StringStruct(::CityLizard::PInvoke::Test::String x)
{
    //::UINT const len = ::SysStringLen(x.def);
    //::std::cout << len << ::std::endl;
    return
        (::wcscmp(x.def, L"def") == 0) &&
        (::strcmp(x.lpstr, "lpstr") == 0) &&
        (::wcscmp(x.lpwstr, L"lpwstr") == 0) &&
        (::wcscmp(x.lptstr, L"lptstr") == 0) &&
        (::wcscmp(x.x, L"::TCHAR[1") == 0) ?
        S_OK :
        E_FAIL;
}

::HRESULT WINAPI MyLPTStr(::LPTSTR s)
{
    return ::wcscmp(s, L"Hello world!") == 0 ? S_OK: E_FAIL;
}

::HRESULT WINAPI AnsiMyLPTStr(::LPTSTR s, ::BSTR s2)
{
    ::UINT const len = ::SysStringLen(s2);
    return 
        (len == 12) &&
        (::wcscmp(s, L"Hello world!") == 0) &&
        (::wcscmp(s2, L"Hello world!") == 0) ? 
        S_OK: 
        E_FAIL;
}

::HRESULT WINAPI CheckBStr(::CityLizard::PInvoke::Test::BStr s)
{
    ::UINT const len = ::SysStringLen(s.A);
    return len == 3 ? S_OK : E_FAIL;
}

::HRESULT WINAPI CheckChars(::CityLizard::PInvoke::Test::Chars chars)
{
    return (chars.A == L'A') && (chars.B == L'B') && (chars.C == L'C') ? S_OK: E_FAIL;
}

::HRESULT WINAPI CheckAnsiChars(::CityLizard::PInvoke::Test::AnsiChars chars)
{
    return (chars.A == 'A') && (chars.B == 'B') && (chars.C == 'C') ? S_OK: E_FAIL;
}

::HRESULT WINAPI CheckArray(::CityLizard::PInvoke::Test::ByValArray x)
{
    for(int i = 0; i < 10; ++i)
    {
        ::std::cout << x.X[i] << ::std::endl;
    }
    return S_OK;
}