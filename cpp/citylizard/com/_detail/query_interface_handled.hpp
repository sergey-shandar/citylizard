#pragma once

#include <citylizard/com/exception.hpp>
#include <citylizard/com/iunknown.hpp>
#include <citylizard/intrusive/ptr.hpp>

namespace citylizard
{
namespace com
{
namespace _detail
{

// See http://msdn.microsoft.com/en-us/library/ms682521(VS.85).aspx.
template<class E, class T>
intrusive::ptr<T> query_interface_handled(::IUnknown &f)
{
    intrusive::ptr<T> r;
    E::throw_if(f.QueryInterface(
        __uuidof(T),
        cast::safe_reinterpret::value(&r.unwrap())));
    return r;
}

}
}
}

