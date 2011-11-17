#pragma once

#include <citylizard/meta/const.hpp>
#include <citylizard/meta/bool_fwd.hpp>

namespace citylizard
{
namespace meta
{

// template typedef
template<bool v>
class bool_: public const_<bool, v>
{
};

typedef bool_<true>::type true_;

typedef bool_<false>::type false_;

}
}
