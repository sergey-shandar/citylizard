#pragma once

#include <citylizard/cast/_detail/base.hpp>

namespace citylizard
{

/// Casting library.
namespace cast
{
namespace _detail
{

class const_
{
public:
    template<class To>
    class traits
    {
    public:
        template<class From>
        static To apply(From const &f)
        {
            return const_cast<To>(f);
        }
    };
}; // class const_

} // namespace _detail

typedef _detail::base<_detail::const_> const_;

} // namespace cast
} // namespace citylizard
