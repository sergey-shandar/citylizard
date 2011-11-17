#pragma once

#include <citylizard/meta/if_c.hpp>
#include <citylizard/meta/bool.hpp>
#include <citylizard/meta/_detail/list_config.hpp>
#include <citylizard/meta/eval_is_same.hpp>

namespace citylizard
{
namespace meta
{

namespace _detail
{

template<class C>
class list;

template<class C>
class list_optional
{
public:

    /// Front.
    typedef typename C::front front;

    /// Back.
    typedef typename C::back back;

    /// Pop front.
    typedef typename list<typename C::pop_front> pop_front;

    /// Pop back.
    typedef typename list<typename C::pop_back> pop_back;

    /// Item.
    template< ::std::size_t I>
    class at_c: public C::template at<I> {};

    /// Item.
    template<class I>
    class at: public at_c<I::value> {};

    /// Erase.
    template< ::std::size_t I>
    class erase_c: public list<typename C::template erase<I>::type> {};

    /// Erase.
    template<class I>
    class erase: public erase_c<I::value>::type {};
};

template<>
class list_optional<list_config::null>
{
};

/// Meta list.
template<class C>
class list: public list_optional<C>
{
public:

    static_assert(
        list_config::is_config<C>::value, "'C' is not a list configuration");

    /// Itself (short form).
    typedef list x;

    /// Itself.
    typedef list type;

	typedef C config;

    /// Equal.
    template<class L>
    class equal: public eval_is_same<L, list> {};

    /// Not equal.
    template<class L>
    class not_equal: public equal<L>::not {};

    /// Size.
    typedef size_t<C::size> size;

    /// Empty.
    typedef bool_<C::size == 0> empty;

    /// Clear.
    typedef list<list_config::null> clear;

    /// Push front.
    template<class T>
    class push_front: public list<typename C::template push_front<T>::type>
    {
    };

    /// Push back.
    template<class T>
    class push_back: public list<typename C::template push_back<T>::type> 
    {
    };

    /// Push front a range.
    template<class R>
    class push_front_range;

    template<class RC>
    class push_front_range<list<RC> >: 
        public list<typename C::template push_front_range<RC>::type>
    {
    };

    /// Push back a range.
    template<class R>
    class push_back_range;

    template<class RC>
    class push_back_range<list<RC> >: 
        public list<typename C::template push_back_range<RC>::type>
    {
    };

    /// Insert.
    template< ::std::size_t I, class T>
    class insert_c: public list<typename C::template insert<I, T>::type> {};

    /// Insert.
    template<class I, class T>
    class insert: public insert_c<I::value, T>::type {};

    /// Insert a range.
    template< ::std::size_t I, class R>
    class insert_range_c;
    
    template< ::std::size_t I, class RC>
    class insert_range_c<I, list<RC> >: 
        public list<typename C::template insert_range<I, RC>::type> 
    {
    };

    /// Insert a range.
    template<class I, class R>
    class insert_range: public insert_range_c<I::value, R> {};

    /// Erase a range.
    template< ::std::size_t IB, ::std::size_t IE>
    class erase_range_c: 
        public list<typename C::template erase_range<IB, IE>::type>
    {
    };

    /// Erase a range.
    template<class IB, class IE>
    class erase_range: public erase_range_c<IB::value, IE::value> {};
};

}

typedef _detail::list<_detail::list_config::null> list;

}
}
