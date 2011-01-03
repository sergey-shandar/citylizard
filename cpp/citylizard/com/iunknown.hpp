#pragma once

#include <citylizard/com/exception.hpp>
#include <citylizard/intrusive/user/traits.hpp>
#include <citylizard/meta/if.hpp>

#include <type_traits>

namespace citylizard_com
{
namespace intrusive
{
namespace user
{

/// COM interface %traits.
template<class T>
class traits<T, typename meta::if_< ::std::is_base_of< ::IUnknown, T> >::type>
{
public:
    static void add(::IUnknown &a) throw()
    {
        a.AddRef();
    }
    static void release(::IUnknown &a) throw()
    {
        a.Release();
    }
    static com::exception bad_ref() throw()
    {
        return com::exception(E_POINTER);
    }
};

}
}
}
