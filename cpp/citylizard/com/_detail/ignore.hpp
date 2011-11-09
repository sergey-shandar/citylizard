#pragma once

#include <windows.h>

namespace citylizard
{
namespace com
{
namespace _detail
{

class ignore
{
public:
    static void throw_if(::HRESULT) throw()
    {
    }
};

}
}
}