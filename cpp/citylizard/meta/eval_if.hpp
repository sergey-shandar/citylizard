#pragma once

#include <citylizard/meta/eval_if_c.hpp>

namespace citylizard_com
{
namespace meta
{

/// \brief Lazy compile time \c if statement by type.
///
/// \sa eval_if_c, if_, if_c.
template<class C, class T, class E = ::boost::mpl::na>
class eval_if:
    public if_c<C::value, typename T::type, typename E::type>
{
};

}
}
