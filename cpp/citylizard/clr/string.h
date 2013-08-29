#pragma once

#include <string>

#include <msclr/marshal.h>
#include <msclr/marshal_cppstd.h>

#include <citylizard/tag.h>

namespace citylizard
{
namespace clr
{
    using namespace std;
    using namespace System;
    using namespace msclr::interop;

    inline string managed_cast(String^ managed, tag<string>)
    {
        return marshal_as<string>(managed);
    }

    inline String^ native_cast(string const &native, tag<String^>)
    {
        return marshal_as<String^>(native);
    }

}
}
