#pragma once

#include <citylizard/tag.h>

namespace citylizard
{
namespace clr
{
    inline bool native_cast(int native, tag<bool>)
    {
        return native != 0;
    }

    inline int managed_cast(bool managed, tag<int>)
    {
        return managed ? 1: 0;
    }
}
}
