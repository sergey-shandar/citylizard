#pragma once

#include <citylizard/meta/is_same_fwd.hpp>
#include <citylizard/meta/bool.hpp>

namespace citylizard
{
namespace meta
{

template<class A, class B>
class is_same: public false_
{
};

template<class A>
class is_same<A, A>: public true_
{
};

}
}