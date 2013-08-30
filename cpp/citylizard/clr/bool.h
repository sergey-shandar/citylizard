#pragma once

#include <citylizard/tag.h>

namespace citylizard
{
namespace clr
{
    // from native int to managed bool.
    inline bool native_cast(int native, tag<bool>)
    {
        return native != 0;
    }

    // from managed bool to native int.
    inline int managed_cast(bool managed, tag<int>)
    {
        return managed ? 1: 0;
    }
}
}
