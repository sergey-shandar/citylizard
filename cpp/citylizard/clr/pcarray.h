#pragma once

#include <vector>

namespace citylizard
{
namespace clr
{
    using namespace std;
    
    template<class T>
    class pcarray_t
    {
    public:

        pcarray_t(array<T>^ managed):
            _native(managed_cast(managed, tag<vector<T>>()))
        {
        }

        operator T const *() const
        {
            return &_native.front();
        }

    private:

        vector<T> const _native;

        pcarray_t &operator=(pcarray_t const &);

    };

    // WARNING: pcarray_y<T>::operator T const *() returns a pointer on a
    // temporary vector.
    template<class T>
    pcarray_t<T> pcarray(array<T>^ managed)
    {
        return pcarray_t<T>(managed);
    }

}
}
