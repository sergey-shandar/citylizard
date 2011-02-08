#pragma once

#include <boost/type_traits/is_base_of.hpp>

#include <citylizard/intrusive/_detail/safe_reinterpret_ref_cast.hpp>

#include <type_traits>

namespace citylizard
{
namespace intrusive
{
namespace _detail
{

template<class To, class From>
To const &down_cast(From const &f)
{
    typedef typename To::element_type to_element_type;
    typedef typename From::element_type from_element_type;
    static_assert(
        (::boost::is_base_of<to_element_type, from_element_type>::value),
        "the type 'To' is not a base class of the type 'From'");
    return safe_reinterpret_ref_cast<To const>(f);
}

}
}
}
