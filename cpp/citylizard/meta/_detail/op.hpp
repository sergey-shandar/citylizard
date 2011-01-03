#pragma once

namespace citylizard_com
{
namespace meta
{
namespace _detail
{

// Workaround for the Doxygen problem:
// bool_<(a < b)>
template<class T, T a, T b>
class less
{
public:
    static bool const value = a < b;
};

// Workaround for the Doxygen problem:
// bool_<(a <= b)>
template<class T, T a, T b>
class less_equal
{
public:
    static bool const value = a <= b;
};

// Workaround for the Doxygen problem:
// bool_<(a << b)>
template<class T, T a, int b>
class left_shift
{
public:
    static T const value = a << b;
};

}
}
}