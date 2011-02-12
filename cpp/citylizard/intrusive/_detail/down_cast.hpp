#pragma once

#include <boost/type_traits/is_base_of.hpp>

#include <citylizard/cast/safe_reinterpret.hpp>

#include <type_traits>

namespace citylizard
{
namespace intrusive
{
namespace _detail
{

template<class Base, class Derived>
Base const &down_cast(Derived const &d)
{
    typedef typename Base::element_type base_t;
    typedef typename Derived::element_type derived_t;
    static_assert(
        (::boost::is_base_of<base_t, derived_t>::value),
        "the type 'Base' is not a base class of the type 'Derived'");
	static_assert(
		sizeof(base_t) == sizeof(derived_t), 
		"sizeof(Base) != sizeof(Derived)");
    return cast::safe_reinterpret::ref(d);
}

}
}
}
