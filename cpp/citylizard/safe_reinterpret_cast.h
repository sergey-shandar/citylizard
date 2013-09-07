#pragma once

#include <citylizard/tag.h>

namespace citylizard
{
    template<class From, class To>
    To safe_reinterpret_cast(From from, tag<To>)
    {
        static_assert(sizeof(From) == sizeof(To), "sizeof(From) != sizeof(To)");
        return *reinterpret_cast<To *>(&from);
    }

    template<class From, class To>
    To *safe_reinterpret_cast(From *from, tag<To *>)
    {
        static_assert(sizeof(From) == sizeof(To), "sizeof(From) != sizeof(To)");
        return reinterpret_cast<To *>(from);
    }

    template<class From>
    class safe_reinterpret_cast_t
    {
    public:

        safe_reinterpret_cast_t(From from): _from(from)
        {
        }

        template<class To>
        operator To() const
        {
            return safe_reinterpret_cast(_from, tag<To>());
        }

    private:

        From const _from;

        safe_reinterpret_cast_t &operator=(safe_reinterpret_cast_t const &);

    };

    template<class From>
    safe_reinterpret_cast_t<From> safe_reinterpret_cast(From from)
    {
        return safe_reinterpret_cast_t<From>(from);
    }

}
