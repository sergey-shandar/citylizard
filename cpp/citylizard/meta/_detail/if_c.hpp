#pragma once

namespace citylizard_com
{
namespace meta
{

namespace _detail
{

template<class T>
class result_t
{
public:
    typedef T type;
    static T type_();
};

template<bool C, class T>
class enable_if_c
{
};

template<class T>
class enable_if_c<true, T>: public result_t<T>
{
};

template<bool C, class T, class E>
class if_c: public result_t<E>
{
};

template<class T, class E>
class if_c<true, T, E>: public result_t<T>
{
};

}
}
}