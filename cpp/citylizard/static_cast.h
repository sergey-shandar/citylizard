#pragma once

namespace citylizard
{
namespace clr
{
    template<class Native>
    class static_cast_t
    {
    public:

        static_cast_t(Native native): _native(native)
        {
        }

        template<class Managed>
        operator Managed() const
        {
            return static_cast<Managed>(_native);
        }

    private:

        Native const _native;

        static_cast_t &operator=(static_cast_t const &);

    };

    template<class Native>
    static_cast_t<Native> static_cast_(Native const &native)
    {
        return static_cast_t<Native>(native);
    }
}
}
