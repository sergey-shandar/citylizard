#define BOOST_TEST_MODULE citylizard
#include <boost/test/unit_test.hpp>

//

#include <citylizard/intrusive/ptr.hpp>
#include <citylizard/com/exception.hpp>
#include <citylizard/com/query_interface.hpp>
#include <citylizard/meta/if.hpp>
#include <Windows.h>

#define override

namespace citylizard
{
namespace com
{

namespace _detail
{

class IUnknown: private ::IUnknown
{
public:
    ::HRESULT __stdcall QueryInterface(const ::IID &,void **) throw() override
    {
        return 0;
    }
    ::ULONG __stdcall AddRef() throw() override
    {
        return 0;
    }
    ::ULONG __stdcall Release() throw() override
    {
        return 0;
    }
};

class traits;

}

class IUnknown
{
public:
    /// The function throws \ref exception "exception(E_NOINTERFACE)" if f has
    /// no T interface.
    /// See http://msdn.microsoft.com/en-us/library/ms682521(VS.85).aspx.
    template<class T>
    intrusive::ptr<T> QueryInterface()
        throw(exception)
    {
        return query_interface<T>(this->_detail);
    }
private:
    IUnknown();
    _detail::IUnknown _detail;
    friend class _detail::traits;
};

namespace _detail
{

class traits
{
public:
    static void add(com::IUnknown &a) throw()
    {
        a._detail.AddRef();
    }
    static void release(com::IUnknown &a) throw()
    {
        a._detail.Release();
    }
    typedef com::exception bad_ref_t;
    static bad_ref_t bad_ref() throw()
    {
        return com::exception(E_POINTER);
    }

    template<class T>
    static T *unwrap(com::IUnknown *p)
    {
        return reinterpret_cast<T *>(&p->_detail);
    }
};

}
}
}

namespace citylizard
{
namespace intrusive
{
namespace user
{

/// COM interface %traits.
template<class T>
class traits<T, typename meta::if_< ::std::is_base_of< com::IUnknown, T> >::type>:
    public com::_detail::traits
{
};

}
}
}

namespace citylizard
{
namespace _detail
{

intrusive::ptr<IUnknown> u;

}
}

class NS_IA: ::IUnknown
{
public:
    virtual ::HRESULT __stdcall A() throw() = 0;
    virtual ::HRESULT __stdcall B(/* in */NS_IA *p) throw() = 0;
};

namespace NS
{

namespace C = ::citylizard::com;
namespace I = ::citylizard::intrusive;
typedef ::citylizard::com::_detail::traits T;

/*
class IA: public ::citylizard_com::com::IUnknown
{
public:
    void A()
    {
        C::exception::throw_if(C::unwrap<NS_IA>(*this).A());
    }
    void B(I::ptr<IA> const &a)
    {
        C::exception::throw_if(T::unwrap<NS_IA>(this)->B(T::unwrap<NS_IA>(a.unwrap())));
    }
};
*/

}
