#pragma once

#include <citylizard/clr/native_cast.h>
#include <citylizard/clr/managed_cast.h>
#include <citylizard/tag.h>

namespace citylizard
{
namespace clr
{   
    using namespace CityLizard::Graphics;

    template<class NativeVector4D, class Managed>
    class native_cast_traits<NativeVector4D, Vector4D<Managed>>
    {
    public:

        static Vector4D<Managed> cast(NativeVector4D native)
        {
            return Vector::New(
                no_cast(native.x),
                no_cast(native.y),
                no_cast(native.z),
                no_cast(native.w));
        }
    };

    template<class Managed, class NativeVector4D>
    class managed_cast_traits<Vector4D<Managed>, NativeVector4D>
    {
    public:

        static NativeVector4D cast(Vector4D<Managed> managed)
        {
            return NativeVector4D(
                no_cast(managed.X),
                no_cast(managed.Y),
                no_cast(managed.Z),
                no_cast(managed.W));
        }
    };

}
}
