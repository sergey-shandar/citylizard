#pragma once

#include <citylizard/com/_detail/query_interface_handled.hpp>
#include <citylizard/com/_detail/ignore.hpp>

namespace citylizard
{
namespace com
{

/// See http://msdn.microsoft.com/en-us/library/ms682521(VS.85).aspx.
/// - never throws an exception. 
/// - returns null if f has no T interface.
template<class T>
intrusive::ptr<T> query_interface_cast(::IUnknown &f) 
    throw()
{
    return _detail::query_interface_handled<_detail::ignore, T>(f);
}

}
}
