#pragma once

namespace citylizard
{
namespace clr
{
    template<class Managed>
    value class managed_static_cast_t
    {
    public:
        
        managed_static_cast_t(Managed managed): _managed(managed)
        {
        }

        template<class Native>
        operator Native()
        {
            return static_cast<Native>(_managed);
        }

    private:

        initonly Managed _managed;

    };

    template<class Managed>
    managed_static_cast_t<Managed> managed_static_cast(Managed managed)
    {
        return managed_static_cast_t<Managed>(managed);
    }

}
}
