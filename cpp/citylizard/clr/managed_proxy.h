#pragma once

#include <citylizard/clr/managed_interface.h>
#include <citylizard/clr/managed_cast.h>

namespace citylizard
{
namespace clr
{
    class ctor {};

    template<class Native, class ManagedBase = Object>
    public ref class managed_proxy abstract:
        ManagedBase, managed_interface<Native>
    {
    private:

        initonly Native *_native;

    internal:

        explicit managed_proxy(Native *native): _native(native)
        {
        }

        explicit managed_proxy(ctor): _native(new Native())
        {
        }

        template<class Param0>
        managed_proxy(ctor, Param0 param0): _native(new Native(param0))
        {
        }

        template<class Param0, class Param1>
        managed_proxy(ctor, Param0 param0, Param1 param1):
            _native(new Native(param0, param1))
        {
        }

        template<class Param0, class Param1, class Param2>
        managed_proxy(ctor, Param0 param0, Param1 param1, Param2 param2):
            _native(new Native(param0, param1, param2))
        {
        }

        template<class Param0, class Param1, class Param2, class Param3>
        managed_proxy(
            ctor, Param0 param0, Param1 param1, Param2 param2, Param3 param3)
        :
            _native(new Native(param0, param1, param2, param3))
        {
        }

    public:

        virtual Native *native(tag<Native>) = managed_interface<Native>::native
        {
            return _native;
        }

        typedef managed_interface<Native> interface_t;

        typedef Native native_t;

        ~managed_proxy()
        {
            this->!managed_proxy();
        }

        !managed_proxy()
        {
            if(_native != nullptr)
            {
                delete _native;
                _native = nullptr;
            }
        }
    };

    template<class Managed, class Native>
    Native *managed_cast(Managed^ managed, tag<Native *>)
    {
        return managed == nullptr ? 
            nullptr:
            static_cast<typename Managed::interface_t^>(managed)->
                native(tag<Native>());
    }

    template<class Managed, class Native>
    Native managed_cast(Managed^ managed, tag<Native>)
    {
        auto const result = managed_cast(managed, tag<Native *>());
        return result == nullptr ? Native(): *result;
    }

}
}
