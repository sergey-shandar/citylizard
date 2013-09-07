#pragma once

#include <citylizard/clr/native_cast.h>
#include <citylizard/clr/managed_cast.h>
#include <citylizard/tag.h>

namespace citylizard
{
namespace clr
{   
    using namespace CityLizard::Graphics;

    template<class NativeVector3D, class Managed>
    class native_cast_traits<NativeVector3D, Vector3D<Managed>>
    {
    public:

        static Vector3D<Managed> cast(NativeVector3D native)
        {
            return Vector::New(
                no_cast(native.x), no_cast(native.y), no_cast(native.z));
        }
    };

    template<class Managed, class NativeVector3D>
    class managed_cast_traits<Vector3D<Managed>, NativeVector3D>
    {
    public:

        static NativeVector3D cast(Vector3D<Managed> managed)
        {
            return NativeVector3D(
                no_cast(managed.X), no_cast(managed.Y), no_cast(managed.Z));
        }
    };

}
}
