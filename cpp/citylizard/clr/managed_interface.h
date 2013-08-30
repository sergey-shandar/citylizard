#pragma once

#include <citylizard/tag.h>

namespace citylizard
{
namespace clr
{
    template<class Native>
    private interface class managed_interface
    {   
        typedef managed_interface interface_t;

        Native *native(tag<Native>);
    };

}
}
