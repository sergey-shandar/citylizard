#include <interface.hpp>

int32_t __stdcall A() { return 0xA; }

::HRESULT __cdecl B(int32_t *p) { *p = 0xB; return S_OK; }

::HRESULT WINAPI C() { return E_FAIL; }

int32_t WINAPI D(uint8_t a, int16_t b) { return a + b; }

::CityLizard::PInvoke::Test::MyEnum WINAPI E(
	::CityLizard::PInvoke::Test::MyEnum a,
	::CityLizard::PInvoke::Test::MyEnum b)
{ 
	return static_cast<::CityLizard::PInvoke::Test::MyEnum>(a - b); 
}
