#include <citylizard/com/is_same_object.hpp>

#include <boost/test/unit_test.hpp>

#pragma warning(disable: 4481)

namespace citylizard
{
namespace com
{
namespace test
{

class My: public ::IUnknown
{
public:
    ::HRESULT __stdcall QueryInterface(const IID &id, void **pp) throw() override 
    {
        if(__uuidof(::IUnknown) == id)
        {
            *pp = (IUnknown *)this;
            return S_OK;
        }
        else
        {
            *pp = nullptr;
            return E_NOINTERFACE;
        }
    }
    ::ULONG __stdcall AddRef() throw() override
    {
        return 1;
    }
    ULONG __stdcall Release() throw() override
    {
        return 1;
    }
};

}
}
}

BOOST_AUTO_TEST_CASE(is_same_object)
{
    namespace C = ::citylizard::com;
    namespace T = ::citylizard::com::test;
    T::My a;
    T::My b;
    BOOST_CHECK(!C::is_same_object(a, b));
    BOOST_CHECK(C::is_same_object(a, a));
}
