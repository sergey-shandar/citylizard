#pragma once

#include <boost/static_assert.hpp>

namespace citylizard_com
{
namespace intrusive
{
namespace _detail
{

template<class To, class From>
To &safe_reinterpret_ref_cast(From &x)
    throw()
{
    static_assert(
        sizeof(To) == sizeof(From), 
        "the size of 'To' must be qual to the size of 'From'");
    return reinterpret_cast<To &>(x);
}

}
}
}
