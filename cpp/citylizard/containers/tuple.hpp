#pragma once

#include <citylizard/meta/list.hpp>

namespace citylizard
{
namespace containers
{

template<class Config>
class tuple_t
{
};

typedef tuple_t<meta::_detail::null> tuple;

}
}