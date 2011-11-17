#pragma once

#include <citylizard/cast/_detail/base.hpp>
#include <boost/cast.hpp>

namespace citylizard
{
namespace cast
{

namespace _detail
{

class polymorphic_down
{
public:

    template<class To>
    class traits;

    template<class To>
    class traits<To *>
    {
    public:
        template<class From>
        static To *apply(From &&f)
        {
            return ::boost::polymorphic_downcast<To *>(f);
        }    
    };

    template<class To>
    class traits<To &>
    {
    public:
        template<class From>
        static To &apply(From &f)
        {
            return *::boost::polymorphic_downcast<To *>(&f);
        }    
    };

}; // class polymorphic_down

} // namespace _detail

typedef _detail::base<_detail::polymorphic_down> polymorphic_down;

} // namespace cast
} // namespace citylizard

