#pragma once

#include <citylizard/clr/native_cast.h>
#include <citylizard/clr/managed_cast.h>

namespace citylizard
{
namespace clr
{
    using namespace CityLizard::Graphics;

    template<class NativeQuaternion, class Managed>
    class native_cast_traits<NativeQuaternion, Quaternion<Managed>>
    {
    public:

        static Quaternion<Managed> cast(NativeQuaternion native)
        {
            return Quaternion::New(native.x, native.y, native.z, native.w);
        }
    };

    template<class Managed, class NativeQuaternion>
    class managed_cast_traits<Quaternion<Managed>, NativeQuaternion>
    {
    public:

        static NativeQuaternion cast(Quaternion<Managed> managed)
        {
            return NativeQuaternion(managed.X, managed.Y, managed.Z, managed.W);
        }
    };

}
}
