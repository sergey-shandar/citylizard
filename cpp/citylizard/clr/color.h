#pragma once

#include <citylizard/no_cast.h>
#include <citylizard/clr/native_cast.h>
#include <citylizard/clr/managed_cast.h>

namespace citylizard
{
namespace clr
{
    using namespace CityLizard::Graphics;

    template<class NativeColor, class Managed>
    class native_cast_traits<NativeColor, Color<Managed>>
    {
    public:

        static Color<Managed> cast(NativeColor native)
        {
            return Color::New(native.r, native.g, native.b, native.a);
        }
    };

    template<class Managed, class NativeColor>
    class managed_cast_traits<Color<Managed>, NativeColor>
    {
    public:

        static NativeColor cast(Color<Managed> managed)
        {
            NativeColor result;
            result.r = no_cast(managed.R);
            result.g = no_cast(managed.G);
            result.b = no_cast(managed.B);
            result.a = no_cast(managed.A);
            return result;
        }
    };    
}
}
