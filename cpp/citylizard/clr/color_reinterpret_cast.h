#pragma once

#include <citylizard/safe_reinterpret_cast.h>
#include <citylizard/tag.h>

namespace citylizard
{
namespace clr
{
    using namespace CityLizard::Graphics;

    template<class From>
    value class color_reinterpret_cast_t
    {
    public:

        explicit color_reinterpret_cast_t(Color<From> from): _from(from) {}

        template<class To>
        operator Color<To>()
        {
            return Color<To>(
                safe_reinterpret_cast(_from.R),
                safe_reinterpret_cast(_from.G),
                safe_reinterpret_cast(_from.B),
                safe_reinterpret_cast(_from.A));
        }

    private:

        initonly Color<From> _from;

    };

    template<class From>
    color_reinterpret_cast_t<From> color_reinterpret_cast(Color<From> from)
    {
        return color_reinterpret_cast_t<From>(from);
    }
}
}
