#pragma once

#include <citylizard/tag.h>

namespace citylizard
{
namespace clr
{
    using namespace System::Runtime::InteropServices;

    template<class Native, class Managed>
    Managed^ native_cast(Native const &native, tag<Managed^>)
    {
        return gcnew Managed(native);
    }

    template<class Managed, class Native>
    ref class native_ptr_t: Managed
    {
    internal:

        explicit native_ptr_t(Native *native): _native(native)
        {
        }

        Native *native(tag<Native>) override
        {
            return _native;
        }

    private:

        initonly Native *_native;

    };

    template<class Native, class Managed>
    Managed^ native_cast(Native *native, tag<Managed^>)
    {
        return gcnew native_ptr_t<Managed, Native>(native);
    }

}
}
