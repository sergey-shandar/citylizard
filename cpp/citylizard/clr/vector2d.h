#pragma once

#include <citylizard/tag.h>

namespace citylizard
{
namespace clr
{   
    using namespace CityLizard::Graphics;

    template<class NativeVector2D, class Managed>
    Vector2D<Managed> native_cast(
        NativeVector2D const &native, tag<Vector2D<Managed>>)
    {
        return Vector2D<Managed>(native.x, native.y)
    }

    template<class Managed, class NativeVector2D>
    NativeVector2D managed_cast(Vector2D<Managed> managed, tag<NativeVector2D>)
    {
        return NativeVector2D(managed.X, managed.Y);
    }

}
}
