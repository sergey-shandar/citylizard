#pragma once

#include <vector>

#include <citylizard/clr/managed_cast.h>

namespace citylizard
{
namespace clr
{
    using namespace std;

    // managed_cast overload from a managed array to a native vector.
    template<class Managed, class Native>
    vector<Native> managed_cast(array<Managed>^ managed, tag<vector<Native>>)
    {
        auto size = managed == nullptr ? 0: managed->Length;
        vector<Native> result(size);
        for(auto i = 0; i < size; ++i)
        {
            result[i] = managed_cast(managed[i], tag<Native>());
        }
        return result;
    }

    template<class Native, class Managed>
    void copy(vector<Native> const &from, array<Managed>^ to)
    {
        auto size = static_cast<int>(from.size());
        for(auto i = 0; i < size; ++i)
        {
            to[i] = native_cast(from[i], tag<Managed>());
        }
    }
}
}
