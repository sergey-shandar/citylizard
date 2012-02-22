#pragma once

#include <boost/utility/enable_if.hpp>
#include <boost/mpl/eval_if.hpp>

#include <citylizard/meta/if_c.hpp>

namespace citylizard
{
namespace meta
{

/// \brief Lazy compile-time \c if statement by value.
///
/// \sa eval_if, if_, if_c.
template<bool C, class T, class E = ::boost::mpl::na>
class eval_if_c: public if_c<C, typename T::type, typename E::type>
{
};

}
}
