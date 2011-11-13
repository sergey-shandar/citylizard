#pragma once

#include <citylizard/meta/is_same.hpp>

namespace citylizard
{
namespace meta
{

template<class A, class B>
class eval_is_same: public is_same<typename A::type, typename B::type>
{
};

}
}