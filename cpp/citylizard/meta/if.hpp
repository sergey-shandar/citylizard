#pragma once

#include <citylizard/meta/eval_if_c.hpp>

namespace citylizard
{
namespace meta
{

/// \brief Compile-time \c if statement by type.
///
/// \sa eval_if, eval_if_c, if_c.
template<class C, class T = void, class E = ::boost::mpl::na>
class if_: public if_c<C::value, T, E>
{
};

}
}
