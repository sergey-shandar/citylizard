#pragma once

#include <citylizard/meta/list.hpp>

namespace citylizard
{
namespace container
{

namespace base
{

template<class Config>
class tuple;

template<class Config>
class tuple_config;

template<>
class tuple_config<meta::_detail::list_config::null>
{
};

template<class Back>
class tuple_config<meta::_detail::list_config::one<Back> >
{
public:

	typedef meta::_detail::list_config::one<Back> config_t;

	Back back_value;

	Back const &front() const
	{
		return this->back_value;
	}

	typedef config_t::clear_t clear_t;

	static clear_t pop_back() const
	{
		return clear_t();
	}

	static clear_t pop_front() const
	{
		return clear_t();
	}

	Back const &at(meta::size_t<0>) const
	{
		return this->back_value;
	}

	clear_t erase(meta::size_t<0>) const
	{
		return clear_t();
	}

	tuple_config(): back_value() {}

	tuple_config(clear_t, Back const &back_value): back_value(back_value) {}
};

template<class PopBack, class Back>
class tuple_config<meta::_detail::list_config::append<PopBack, Back> >
{
public:

	typedef meta::_detail::list_config::append<PopBack, Back> config_t;

	static ::std::size_t const size = Config::size;

	Back back_value;

	tuple<PopBack> pop_back_value;

	typename PopBack::front const &front() const
	{
		return this->pop_back_value.front();
	}

	tuple<PopBack> pop_back() const
	{
		return this->pop_back_value();
	}

	typedef tuple<typename config_t::pop_front> pop_front_t;

	pop_front_t pop_front() const
	{
		return pop_front_t(this->pop_back_value.pop_front(), this->back_value);
	}

	template< ::std::size_t I>
	typename Config::template at<I>::type const &at(meta::size_t<I>) const
	{
		return this->pop_back_value.template at<I>();
	}

	Back const &at(meta::size_t<size>) const
	{
		return this->back_value;
	}

	template< ::std::size_t I>
	typename Config::template erase<I>::type erase(meta::size_t<I>) const
	{
		typedef typename Config::template erase<I>::type erase_t;
		return erase_t(this->pop_back_value.template erase<I>(), this->back_value);
	}

	PopBack const &erase(meta::size_t<size>) const
	{
		return this->pop_back_value;
	}

	tuple_config(): pop_back_value(), back_value() {}

	tuple_config(tuple<PopBack> const &pop_back_value, Back const &back_value):
		pop_back_value(pop_back_value), back_value(back_value)
	{
	}
};

template<class Config>
class tuple_optional
{
public:

	typedef typename Config::back back_t;

	back_t const &back() const
	{
		return this->_detail.back_value;
	}

	typedef typename Config::front front_t;

	front_t const &front() const
	{
		return this->_detail.front();
	}

	typedef tuple<typename Config::pop_back> pop_back_t;

	pop_back_t pop_back() const
	{
		return this->_detail.pop_back();
	}

	typedef tuple<typename Config::pop_front> pop_front_t;

	pop_front_t pop_front() const
	{
		return this->_detail.pop_front();
	}

	template< ::std::size_t I>
	class at_t: public Config_t::template at<I> {};

	template< ::std::size_t I>
	typename at_t<I>::type const &at() const
	{
		return this->_detail.at(meta::size_t<I>());
	}

	template<class I>
	typename at_t<I::value>::type const &operator[](I) const
	{
		return this->_detail.at(meta::size_t<I::value>());
	}

	template< ::std::size_t I>
	class erase_t: public tuple<typename Config_t::template erase<I>::type> {};

	template< ::std::size_t I>
	typename erase_t<I>::type erase() const
	{
		return this->_detail.erase(meta::size_t<I>());
	}

	template<I>
	typename erase_t<I::value>::type erase(I) const
	{
		return this->_detail.erase(meta::size_t<I::value>());
	}

	tuple_optional() {}

	tuple_optional(pop_back_t const &pop_back_value, back_t const &back_value):
		_detail(pop_back_value, back_value)
	{
	}

protected:

	tuple_config<Config> _detail;

};

template<>
class tuple_optional<meta::_detail::list_config::null>
{
};

template<class Config>
class tuple: public tuple_optional<Config>
{
private:

	typedef tuple_optionale<Config> optional_t;

public:

	tuple() {}

	tuple(
		tuple<typename Config::pop_back> const &pop_back_value, 
		typename Config::back const &back_value)
	:
		optional_t(pop_back_value, back_value)
	{
	}
};

}

typedef base::tuple<meta::_detail::list_config::null> tuple;

}
}