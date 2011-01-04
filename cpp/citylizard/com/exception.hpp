#pragma once

#include <exception>

#include <windows.h>

#pragma warning(push)
// C++ exception specification ignored except to indicate a function is not
// __declspec(nothrow)
#pragma warning(disable: 4290)
// nonstandard extension used: override specifier 'keyword'
#pragma warning(disable: 4481)

namespace citylizard
{
/// COM (Component Object Model).
namespace com
{

/// COM %exception.
class exception: public ::std::exception
{
public:
    ::HRESULT const hresult;
    explicit exception(::HRESULT x)
        throw():
        hresult(x)
    {
    }
    static void throw_if(::HRESULT x) 
        throw(exception)
    {
        if(x != S_OK)
        {
            throw exception(x);
        }
    }
private:
    char const *what() const throw() override
    {
        return "citylizard_com::com::exception";
    }
    exception &operator=(exception const &);
};

}
}

#pragma warning(pop)
