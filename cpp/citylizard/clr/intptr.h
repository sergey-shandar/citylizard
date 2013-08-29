#pragma once

#include <citylizard/clr/managed_static_cast.h>
#include <citylizard/static_cast.h>
#include <citylizard/tag.h>

namespace citylizard
{
namespace clr
{
    using namespace System;

    inline void *managed_cast(IntPtr managed, tag<void *>)
    {
        return managed_static_cast(managed);
    }

    template<class Native>
    inline Native *managed_cast(IntPtr managed, tag<Native *>)
    {
        return static_cast_(managed_cast(managed, tag<void *>()));
    }

    inline IntPtr native_cast(void *native, tag<IntPtr>)
    {
        return static_cast_(native);
    }

    template<class Native>
    inline IntPtr native_cast(Native *native, tag<IntPtr>)
    {
        return native_cast(static_cast<void *>(native));
    }

    template<class Native>
    inline IntPtr native_cast(Native const *native, tag<IntPtr>)
    {
        return native_cast(const_cast<Native *>(native));
    }
}
}
