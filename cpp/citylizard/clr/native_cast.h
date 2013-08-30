#pragma once

#include <citylizard/tag.h>

namespace citylizard
{
namespace clr
{

    // enumerations.
    template<class Native, class Managed>
    Managed native_cast(Native const &native, tag<Managed>)
    {
        typedef typename std::underlying_type<Managed>::type underlying_t;
        static_assert(is_same<underlying_t, Native>::value, "invalid cast");
        return static_cast<Managed>(native);
    }

    // no cast.
    template<class Native>
    Native native_cast(Native const &native, tag<Native>)
    {
        return native;
    }

    // holder.
    template<class Native>
    class native_cast_t
    {
    public:

        native_cast_t(Native native): _native(native)
        {
        }

        template<class Managed>
        operator Managed() const
        {
            return native_cast(_native, tag<Managed>());
        }

    private:

        Native const _native;

        native_cast_t &operator=(native_cast_t const &);

    };

    // native_cast with type inference
    template<class Native>
    native_cast_t<Native> native_cast(Native native)
    {
        return native_cast_t<Native>(native);
    }

}
}
