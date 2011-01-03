#pragma once

#include <citylizard/com/_detail/query_interface_handled.hpp>
#include <citylizard/com/exception.hpp>

#pragma warning(push)
// C++ exception specification ignored except to indicate a function is not
// __declspec(nothrow)
#pragma warning(disable: 4290)

namespace citylizard_com
{
namespace com
{

/// The function throws \ref exception "exception(E_NOINTERFACE)" if f has no T
/// interface.
/// See http://msdn.microsoft.com/en-us/library/ms682521(VS.85).aspx.
template<class T>
intrusive::ptr<T> query_interface(::IUnknown &f)
    throw(exception)
{
    return _detail::query_interface_handled<exception, T>(f);
}

}
}

#pragma warning(pop)
