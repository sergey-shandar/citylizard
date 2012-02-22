#pragma once

#include <citylizard/cast/_detail/base.hpp>

namespace citylizard
{

/// Casting library.
namespace cast
{
namespace _detail
{

class implicit
{
public:
    template<class To>
    class traits
    {
    public:
        template<class From>
        static To apply(From &&f)
        {
            return f;
        }
    };
}; // class const_

} // namespace _detail

typedef _detail::base<_detail::implicit> implicit;

} // namespace cast
} // namespace citylizard
