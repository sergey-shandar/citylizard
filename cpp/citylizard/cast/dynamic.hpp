#pragma once

#include <citylizard/cast/_detail/base.hpp>

namespace citylizard
{
namespace cast
{

namespace _detail
{

class dynamic: public base<dynamic>
{
public:
    template<class To>
    class traits
    {
    public:
        template<class From>
        static To apply(From &&f)
        {
            return dynamic_cast<To>(f);
        }
    };
}; // class dynamic

} // namespace _detail

typedef _detail::base<_detail::dynamic> dynamic;

} // namespace cast
} // namespace citylizard

