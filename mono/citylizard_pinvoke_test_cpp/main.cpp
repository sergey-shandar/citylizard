#include <interface.hpp>

HRESULT A(::CityLizard::PInvoke::Test::MyStruct)
{
	return S_OK;
}

HRESULT C(int32_t *)
{
	return S_OK;
}

HRESULT D(int32_t, int32_t, wchar_t, int32_t *)
{
	return S_OK;
}

HRESULT GetInterface(BSTR *, ::CityLizard::PInvoke::Test::IMyInterface *)
{
	return S_OK;
}
