#pragma once

#include <citylizard/com/query_interface_cast.hpp>

namespace citylizard
{
namespace com
{

/// Returns true if a and b reference to the same object.
/// See http://msdn.microsoft.com/en-us/library/ms682521(VS.85).aspx.
/// - "For any one object, a specific query for the IUnknown interface on any of 
/// the object's interfaces must always return the same pointer value."
/// - "It must be reflexive - if a client holds a pointer to an interface on an
/// object, and queries for that interface, the call must succeed."
inline bool is_same_object(::IUnknown &a, ::IUnknown &b) 
    throw()
{
    if(&a == &b)
    {
        return true;
    }
    auto const pa = query_interface_cast< ::IUnknown>(a);
    if(pa.unwrap() == &b)
    {
        return true;
    }
    return pa == query_interface_cast< ::IUnknown>(b);
}

}
}