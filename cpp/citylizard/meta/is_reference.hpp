#pragma once

#include <citylizard/meta/bool.hpp>

namespace citylizard
{
namespace meta
{

template<class T>
struct is_reference: false_ 
{
};

template<class T>
struct is_reference<T &>: true_ 
{
};

}
}
