#pragma once

#include <citylizard/tag.h>
#include <citylizard/static_cast.h>

namespace citylizard
{
namespace clr
{
    using namespace System;

    inline Char native_cast(int native, tag<Char>)
    {
        return static_cast_(native);
    }

    inline int managed_cast(Char managed, tag<int>)
    {
        return static_cast_(managed);
    }
}
}
