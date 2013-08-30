#pragma once

#include <citylizard/clr/string.h>

namespace citylizard
{
namespace clr
{
    
    class pcstr_t
    {
    public:
        
        pcstr_t(String^ managed): 
            _native(managed_cast(managed, tag<string>()))
        {
        }

        operator char const *() const
        {
            return _native.c_str();
        }

    private:

        string const _native;

        pcstr_t &operator=(pcstr_t const &);
    };

    // WARNING: pcstr_t::operator char const *() returns a pointer on a 
    // temporary string.
    inline pcstr_t pcstr(String^ managed)
    {
        return pcstr_t(managed);
    }

    inline String^ native_cast(char const *native, tag<String^>)
    {
        return marshal_as<String^>(native);
    }

    inline String^ native_cast(char *native, tag<String^>)
    {
        return marshal_as<String^>(native);
    }
}
}
