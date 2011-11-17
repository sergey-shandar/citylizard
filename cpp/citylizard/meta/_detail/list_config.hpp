#pragma once

#include <citylizard/meta/if_c.hpp>
#include <citylizard/meta/bool.hpp>

#include <boost/mpl/identity.hpp>

namespace citylizard
{
namespace meta
{
namespace _detail
{

template<class C>
class tuple;

/// Requirements to list config:
///
/// - 'front' value type (optional).
/// - 'back' value type (optional).
///
/// - 'pop_front' list type (optional).
/// - 'pop_back' list type (optional).
///
/// - 'at' value template typedef (optional).
///
/// - 'erase' value template typedef (optional).
///
/// - 'erase_range' list template typedef.
///
/// - 'size' int type.
/// 
/// - 'push_front' list template typedef.
/// - 'push_back' list template typedef.
///
/// - 'push_front_range' list template typedef.
/// - 'push_back_range' list template typedef.
///
/// - 'insert' list template typedef.
///
/// - 'insert_range' list template typedef.
namespace list_config
{

template<class T>
class one;

/// empty list config
class null
{
public:

    typedef null type;

    static ::std::size_t const size = 0;

    /// {T}
    template<class T>
    class push_front: public one<T> {};

    /// {T}
    template<class T>
    class push_back: public one<T> {};

    /// R
    template<class R>
    class push_front_range: public R {};

    /// R
    template<class R>
    class push_back_range: public R {};

    template< ::std::size_t I, class T>
    class insert;

    template<class T>
    class insert<0, T>: public one<T> {};

    template< ::std::size_t I, class R>
    class insert_range;

    template<class R>
    class insert_range<0, R>: public R {};

    template< ::std::size_t IB, ::std::size_t IE>
    class erase_range;

    template<>
    class erase_range<0, 0>    
    {
    public:
        typedef null type;
    };

};

template<class C, class B>
class append;

template<class B>
class one
{
public:

    typedef one type;

    typedef B front;
    
	typedef B back;
    
	typedef null pop_front;
	typedef tuple<null> pop_front_tuple;

    typedef null pop_back;
	typedef tuple<null> pop_back_tuple;

    static ::std::size_t const size = 1;

    // {T}{B} => append(one(T), B)
    template<class T>
    class push_front: public append<one<T>, B> {};

    // {B}{T} => append(one(B), T)
    template<class T>
    class push_back: public append<type, T> {};

    /// R{B} => R.push_back(B)
    template<class R>
    class push_front_range: public R::template push_back<B>::type {};

    /// {B}R
    template<class R>
    class push_back_range: public R::template push_front<B>::type {};

    template< ::std::size_t I>
    class at;

    template<>
    class at<0>: public boost::mpl::identity<B> {};

    template< ::std::size_t I>
    class erase;
    
    template<>
    class erase<0>: public null {};

    template< ::std::size_t I, class T>
    class insert;

    /// {T}{B}
    template<class T>
    class insert<0, T>: public append<one<T>, B> {};

    /// {B}{T}
    template<class T>
    class insert<1, T>: public append<type, T> {};

    template< ::std::size_t I, class R>
    class insert_range;

    /// R{B}
    template<class R>
    class insert_range<0, R>: public R::template push_back<B>::type {};

    /// {B}R
    template<class R>
    class insert_range<1, R>: public R::template push_front<B>::type {};

    template< ::std::size_t IB, ::std::size_t IE>
    class erase_range;

    template<>
    class erase_range<0, 0>
    {
    public:
        typedef type type;
    };

    template<>
    class erase_range<0, 1>: public null {};

    template<>
    class erase_range<1, 1>
    {
    public:
        typedef type type;
    };

	class tuple_detail
	{
	public:

		back back_v;

		const front &front_ref() const
		{
			return this->back_value;
		}

		static pop_back_tuple pop_back_value()
		{
			return tuple<null>();
		}

		template< ::std::size_t I>
		const typename at_c<I>::type &at_value() const
		{
			return this->back_v;
		}

	};

};

/// C must be either 'one' or 'append'.
template<class C, class B>
class append
{
public:

    static_assert(
        !is_same<C, null>::value, 
        "'C' can not be a null list configuration.");

    typedef append type;

    typedef C pop_back;
	typedef tuple<pop_back> pop_back_tuple;

    typedef B back;

    typedef typename pop_back::pop_front::template push_back<B>::type
        pop_front;
	typedef tuple<pop_front> pop_front_tuple;

    typedef typename pop_back::front front;

    static ::std::size_t const size = pop_back::size + 1;

    /// {T}C{B}
    template<class T>
    class push_front: public append<typename C::template push_front<T>::type, B>
    {
    };

    /// C{B}{T}
    template<class T>
    class push_back: public append<type, T> {};

    /// RC{B}
    template<class R>
    class push_front_range: 
        public append<typename C::template push_front_range<R>::type, B>
    {
    };

    /// C{B}R
    template<class R>
    class push_back_range: public R::template push_front_range<type>::type {};

    template< ::std::size_t I>
    class at: public C::template at<I> {};

    template<>
    class at<size - 1>: public ::boost::mpl::identity<B> {};

    template< ::std::size_t I>
    class erase: public C::template erase<I>::type::template push_back<B> {};
    
    template<>
    class erase<size - 1>: public C {};

    /// append(C.insert(I, T), B)
    template< ::std::size_t I, class T>
    class insert: public append<typename C::template insert<I, T>::type, B> {};

    /// C{B}{T}
    template<class T>
    class insert<size, T>: public append<type, T> {};

    /// append(C.insert_range(I, R), B)
    template< ::std::size_t I, class R>
    class insert_range: 
        public append<typename C::template insert_range<I, R>::type, B>
    {
    };

    /// C{B}R
    template<class R>
    class insert_range<size, R>: public R::template push_front_range<type>::type
    {
    };

    template< ::std::size_t IB, ::std::size_t IE>
    class erase_range: 
        public C::template erase_range<IB, IE>::template push_back<B>::type
    {
    };

    template< ::std::size_t IB>
    class erase_range<IB, size>: 
        public C::template erase_range<IB, size - 1>::type
    {
    };

    template<>
    class erase_range<size, size>
    {
    public:
        typedef type type;
    };

	class tuple_detail
	{
	public:

		pop_back_tuple pop_back_v;
		
		back back_v;

		const front &front_ref() const
		{
			return this->pb.front();
		}

		const pop_back_tuple &pop_back_value() const
		{
			return this->pop_back_v;
		}

		pop_front_tuple pop_front_value() const
		{
			return this->pop_back_v.pop_front().push_back(this->back_v);
		}

		template< ::std::size_t I>
		const typename at_c<I>::type &at_value() const
		{
			return this->pop_back_v.at<I>();
		}

		template<>
		const typename at_c<size - 1>::type &at_value<size - 1>() const
		{
			return this->back_v;
		}
	};

};

template<class Config>
class is_config: public meta::false_ {};

template<>
class is_config<null>: public meta::true_ {};

template<class T>
class is_config<one<T> >: public meta::true_ {};

template<class C, class B>
class is_config<append<C, B> >: public meta::true_ {};

}
}
}
}