#pragma once

#include <citylizard/clr/managed_static_cast.h>
#include <citylizard/static_cast.h>
#include <citylizard/tag.h>

namespace citylizard
{
namespace clr
{
    using namespace System;

    // from managed IntPtr to native pointer on void.
    inline void *managed_cast(IntPtr managed, tag<void *>)
    {
        return managed_static_cast(managed);
    }

    // from managed IntPtr to a native pointer. 
    template<class Native>
    Native *managed_cast(IntPtr managed, tag<Native *>)
    {
        return static_cast_(managed_cast(managed, tag<void *>()));
    }

    // from native pointer on void to managed IntPtr.
    inline IntPtr native_cast(void *native, tag<IntPtr>)
    {
        return static_cast_(native);
    }

    // from native pointer on void to managed IntPtr.
    template<class Native>
    IntPtr native_cast(Native *native, tag<IntPtr>)
    {
        return native_cast(static_cast<void *>(native));
    }

    // from native pointer to managed IntPtr.
    template<class Native>
    IntPtr native_cast(Native const *native, tag<IntPtr>)
    {
        return native_cast(const_cast<Native *>(native));
    }
}
}
