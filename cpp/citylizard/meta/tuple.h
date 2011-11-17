#pragma once

#include <citylizard/meta/list.hpp>

namespace citylizard
{
namespace meta
{

namespace _detail
{

template<class C>
class tuple_optional
{
public:

	typedef typename C::front front_t;

	const front_t &front() const
	{
		return this->_detail.front_ref();
	}

	typedef typename C::back back_t;

	const back_t &back() const
	{
		return this->_detail.back_v;
	}

	typedef typename C::pop_back_tuple pop_back_t;

	pop_back_t pop_back() const
	{
		return this->_detail.pop_back_value();
	}

	template< ::std::size_t I>
	class at_t: public C::at_c<I> { };

	template< ::std::size_t I>
	const typename at_t<I>::type &at() const
	{
		return this->_detail.at_value<I>();
	}

private:

	typename C::tuple _detail;

};

template<>
class tuple_optional<list_config::null>
{
public:
};

template<class C>
class tuple: public tuple_optional<C>
{
public:

    /// Itself (short form).
    typedef list x;

    /// Itself.
    typedef list type;

	/// meta list.
	typedef list<C> list_t;

	typedef ::std::size_t size_type;

	static size_type const size = list_t::size::value;

	static bool const empty = list_t::empty::value;

	static tuple<list_config::null> clear()
	{
		return tuple<list_config::null>();
	}
};

}

typedef _detail::tuple<_detail::list_config::null> tuple;

}
}