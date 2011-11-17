#pragma once

#include <citylizard/cast/_detail/base.hpp>
#include <boost/cast.hpp>

namespace citylizard
{
namespace cast
{

namespace _detail
{

class polymorphic
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
            return ::boost::polymorphic_cast<To *>(f);
        }
    };

    template<class To>
    class traits<To &>
    {
    public:
        template<class From>
        static To &apply(From &f)
        {
            return *::boost::polymorphic_cast<To *>(&f);
        }    
    };

}; // class polymorphic

} // namespace _detail

typedef _detail::base<_detail::polymorphic> polymorphic;

} // namespace cast
} // namespace citylizard

