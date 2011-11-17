#pragma once

#include <citylizard/meta/if.hpp>
#include <citylizard/meta/is_reference.hpp>

namespace citylizard
{
namespace cast
{
namespace _detail
{

template<class C>
class base
{
public:

    template<class T>
    class to
    {
    public:
        template<class F>
        static T apply(F const &f)
        {
            return C::template traits<T>::apply(f);
        }
    };

    template<class T>
    class to<T &>
    {
    public:
        template<class F>
        static T &apply(F &f)
        {
            return C::template traits<T &>::apply(f);
        }
    };

    template<class F>
    class value_t
    {
    public:

        explicit value_t(F const &f):
            f(f)
        {
        }

        explicit value_t(F &&f):
            f(f)
        {
        }

        template<class T>
        operator T() const
        {
            static_assert(
                !::boost::is_reference<T>::value, 
                "type T is a reference type.");
            return C::template traits<T>::apply(static_cast<F &&>(this->f));
        }

    private:

        mutable F f;

        value_t &operator=(value_t const &);

    }; // class value_t

    template<class F>
    static value_t<F> value(F const &f)
    {
        return value_t<F>(f);
    }

    template<class F>
    static typename meta::if_c<meta::is_reference<F>::not_v, value_t<F> >::type 
        value(F &&f)
    {
        return value_t<F>(f);
    }

    template<class F>
    class ref_t
    {
    public:

        explicit ref_t(F &f):
            f(f)
        {
        }

        template<class T>
        operator T &() const
        {
            return C::template traits<T &>::apply(this->f);
        }

    private:

        F &f;

        ref_t &operator=(ref_t const &);

    }; // class ref_t

    template<class F>
    static ref_t<F> ref(F &f)
    {
        return ref_t<F>(f);
    }

}; // class base

} // namespace _detail
} // namespace cast
} // namespace citylizard

