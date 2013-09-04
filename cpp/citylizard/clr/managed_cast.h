#pragma once

#include <citylizard/tag.h>

namespace citylizard
{
namespace clr
{
    template<class Managed, class Native>
    class managed_cast_traits
    {
    public:
    
        static Native cast(Managed managed)
        {
            typedef typename std::underlying_type<Managed>::type underlying_t;
            static_assert(is_same<underlying_t, Native>::value, "invalid cast");
            return static_cast<Native>(managed);            
        }
    };

    template<class Managed, class Native>
    Native managed_cast(Managed managed, tag<Native>)
    {
        return managed_cast_traits<Managed, Native>::cast(managed);
    }

    template<class Managed>
    Managed managed_cast(Managed managed, tag<Managed>)
    {
        return managed;
    }

    template<class Managed>
    value class managed_cast_t
    {
    public:

        managed_cast_t(Managed managed): _managed(managed)
        {
        }

        template<class Native>
        operator Native()
        {
            auto const value = managed_cast(_managed, tag<Native>());
            // usually means that there is no better overload found.
            static_assert(
                is_same<decltype(value), Native const>::value,
                "decltype(value) != Native");
            return value;
        }

    private:

        initonly Managed _managed;

    };

    template<class Managed>
    managed_cast_t<Managed> managed_cast(Managed managed)
    {
        return managed_cast_t<Managed>(managed);
    }

}
}
