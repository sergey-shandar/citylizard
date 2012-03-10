#include <interface.hpp>

static_assert(::CityLizard::PInvoke::Test::MyBigEnum::MaxValue == LLONG_MAX, "xxx");
static_assert(::CityLizard::PInvoke::Test::MyBigEnum::MinValue == LLONG_MIN, "xxx");
static_assert(::CityLizard::PInvoke::Test::MyPBigEnum::MaxValue == ULLONG_MAX, "xxx");

int32_t __stdcall A() { return 0xA; }

::HRESULT __cdecl B(int32_t *p) { *p = 0xB; return S_OK; }

::HRESULT WINAPI C() { return E_FAIL; }

int32_t WINAPI D(uint8_t a, int16_t b) { return a + b; }

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

::HRESULT WINAPI CheckBool(BOOL a, BOOL b, BOOL c, BOOL d, int32_t x)
{
    return 
        (((x >> 12) & 1) == a) && 
        (((x >>  8) & 1) == b) &&
        (((x >>  4) & 1) == c) &&
        (((x >>  0) & 1) == d) ?
        S_OK:
        E_FAIL;
}

::SAFEARRAY *p = 0;