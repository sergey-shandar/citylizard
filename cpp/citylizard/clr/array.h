#pragma once

#include <vector>

#include <citylizard/clr/managed_cast.h>

namespace citylizard
{
namespace clr
{
    using namespace std;

    template<class Managed, class NativeVector>
    NativeVector managed_cast(array<Managed>^ managed, tag<NativeVector>)
    {
        auto size = managed == nullptr ? 0: managed->Length;
        NativeVector result(size);
        typedef typename remove_reference<decltype(result[0])>::type Native;
        for(auto i = 0; i < size; ++i)
        {
            result[i] = managed_cast(managed[i], tag<Native>());
        }
        return result;
    }

    template<class NativeVector, class Managed>
    void copy(NativeVector const &from, array<Managed>^ to)
    {
        auto size = static_cast<int>(from.size());
        for(auto i = 0; i < size; ++i)
        {
            to[i] = native_cast(from[i], tag<Managed>());
        }
    }

}
}
