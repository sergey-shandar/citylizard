#pragma once

#include <citylizard/meta/list.hpp>

namespace citylizard
{
namespace tuple
{

namespace _detail
{

template<class C>
class list_optional
{
    /// Front type.
    typedef typename C::front front_t;

	/// Front.
	front_t const &front() const
	{
	}

	/// Front.
	front_t &front()
	{
	}

    /// Back type.
    typedef typename C::back back_t;

	/// Back.
	back_t const &back() const
	{
	}

	/// Back.
	back_t &back()
	{
	}

    /// Pop front type.
    typedef typename list<typename C::pop_front> pop_front_t;

	/// Pop front.
	pop_front_t pop_front() const
	{
	}

    /// Pop back type.
    typedef typename list<typename C::pop_back> pop_back_t;

	pop_back_t pop_back() const
	{
	}

    /// Item type.
    template< ::std::size_t I>
    class at_t: public C::template at<I> {};

	/// Item.
	template< ::std::size_t I>
	const typename at_t<I>::type &at() const
	{
	}

	/// Item.
	template< ::std::size_t I>
	typename at_t<I>::type &at()
	{
	}

    /// Erase type.
    template< ::std::size_t I>
    class erase_t: public list<typename C::template erase<I>::type> 
	{
	};

    /// Erase.
    template<class I>
    typename erase_t<I>::type erase() 
	{
	}

};

template<>
class list_optional<meta::_detail::list_config::null>
{
};

/// Tuple list.
template<class C>
class list: public list_optional<C>
{
public:

	/// Meta list.
	typedef meta::detail_::list<C> list_t;

    /// Itself.
    typedef list type;

    /// Size.
    static ::std::size_t const size = C::size;

    /// Empty.
    static ::std::size_t const empty = C::empty;

	/// Clear type.
	typedef typename list_t::clear clear_t;

    /// Clear.
    static clear_t clear()
	{
		return clear_t();
	}

    /// Push front type.
    template<class T>
    class push_front_t: public list_t::template push_front<T> { };

	/// Push front.
	template<class T>
	typename push_front_t<T>::type push_front(T const &t) const
	{
	}

	/// Push back type.
	template<class T>
	class push_back_t: public list_t::template push_back<T> { };

	/// Push back.
	template<class T>
	typename push_back_t<T>::type push_back(T const &t) const
	{
	}

	/// Push front range type.
	template<class L>
	class push_front_range_t;
	
	template<class C>
	class push_front_range_t<list<C> >:
		public list_t::template push_front_range<meta::_detail::list<C> > 
	{ 
	};

	/// Push front range.
	template<class C>
	typename push_front_range_t<list<C> >::type 
		push_front_range(list<C> const &list) const
	{
	}

	/// Push back range type.
	template<class L>
	class push_back_range_t;
	
	template<class C>
	class push_back_range_t<list<C> >: 
		public list_t::template push_back_range<meta::_detail::list<C> >
	{
	};

	/// Push back range.
	template<class C>
	typename push_back_range_t<list<C> >::type 
		push_back_range(list<C> const &list) const
	{
	}

    /// Insert type.
    template< ::std::size_t I, class T>
    class insert_t: public list_t::template insert_c<I, T> { };

	/// Insert.
	template< ::std::size_t I, class T>
	typename insert_t<I, T>::type insert(T const &t) const
	{
	}

	template< ::std::size_t I, class L>
	class insert_range_t;
	
	/// Insert range type.
	template< ::std::size_t I, class C>
	class insert_range_t<I, list<C> >:
		public list_t::template insert_range_c<I, meta::_detail::list<C> >
	{
	};

	/// Insert range.
	template< ::std::size_t, class C>
	typename insert_range_t<I, list<C> > insert_range(list<T> const &list) const
	{
	}

	/// Erase range type.
	template< ::std::size_t IB, ::std::size_t IE>
	class erase_range_t: public list_t::template erase_range_c<IB, IE> { };

	/// Erase range.
	template< ::std::size_t IB, ::std::size_t IE>
	erase_range_t<IB, IE> erase_range() const
	{
	}
};

}

typedef _detail::list<meta::_detail::list_config::null> list;

}
}