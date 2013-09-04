#pragma once

#include <citylizard/clr/native_cast.h>
#include <citylizard/clr/managed_cast.h>
#include <citylizard/tag.h>

namespace citylizard
{
namespace clr
{   
    using namespace CityLizard::Graphics;

    template<class NativeVector2D, class Managed>
    class native_cast_traits<NativeVector2D, Vector2D<Managed>>
    {
    public:

        static Vector2D<Managed> cast(NativeVector2D native)
        {
            return Vector::New(native.x, native.y);
        }
    };

    template<class Managed, class NativeVector2D>
    class managed_cast_traits<Vector2D<Managed>, NativeVector2D>
    {
    public:

        static NativeVector2D cast(Vector2D<Managed> managed)
        {
            return NativeVector2D(managed.X, managed.Y);
        }
    };    

}
}
