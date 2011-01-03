#pragma once

#include <citylizard/com/exception.hpp>
#include <citylizard/com/iunknown.hpp>
#include <citylizard/intrusive/ptr.hpp>

namespace citylizard_com
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
        &intrusive::_detail::safe_reinterpret_ref_cast<void *>(r.unwrap())));
    return r;
}

}
}
}

