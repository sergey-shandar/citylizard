#pragma once

#include <citylizard/meta/list.hpp>
#include <citylizard/meta/const.hpp>

namespace citylizard
{
namespace tuple
{

namespace _detail
{

template<class C>
class list;

template<class C>
class list_struct;

template<>
class list_struct<meta::_detail::list_config::null>
{
public:

	typedef meta::_detail::list_config::null config_t;

	template<class T>
	static list<typename config_t::push_back<T>::type> push_back(T const &value)
	{
		return list<config_t::push_back<T>::type>(value);
	}
};

template<class Back>
class list_struct<meta::_detail::list_config::one<Back> >
{
public:

	Back back_value;

	Back const &front() const
	{
		return this->back_value;
	}

	typedef meta::_detail::list_config::one<Back> config_t;

	typedef list<typename config_t::clear> clear_t;

	static clear_t pop_back()
	{
		return clear_t();
	}

	static clear_t pop_front()
	{
		return clear_t();
	}

	Back at(meta::size_t<0>) const
	{
		return this->back_value;
	}

	static clear_t erase(meta::size_t<0>)
	{
		return clear_t();
	}

	template<class T>
	list<typename config_t::template push_back<T>::type> 
	push_back(T const &value) const
	{
		return list<typename config_t::template push_back<T>::type>(*this, value);
	}
};

template<class PopBack, class Back>
class list_struct<meta::_detail::list_config::append<PopBack, Back> >
{
private:

	typedef meta::_detail::list_config::append<PopBack, Back> config_t;

	list<PopBack> pop_back_value;

public:

	static ::std::size_t const size = config_t::size;

	Back back_value;

	typename PopBack::front const &front() const
	{
		return this->pop_back_value.front(); 
	}

	list<PopBack> const &pop_back() const
	{
		return this->pop_back_value;
	}

	list<typename config_t::pop_front> pop_front() const
	{
		return this->pop_back_value.pop_front().push_back(this->back_value);
	}

	template< ::std::size_t I>
	typename config_t::template at<I>::type at(meta::size_t<I>) const
	{
		this->pop_back_value.template at<I>();
	}

	Back const &at(meta::size_t<size>) const
	{
		return this->back_value;
	}

	template< ::std::size_t I>
	list<typename config_t::template erase<I>::type> erase(meta::size_t<I>) const
	{
		this->pop_back.erase<I>().push_back(this->back_value);
	}

	PopBack const &erase(meta::size_t<size>) const
	{
		return this->pop_back;
	}

	template<class T>
	list<typename config_t::template push_back<T>::type>
	push_back(T const &value) const
	{
		return list<typename config_t::template push_back<T>::type>(*this, value);
	}
};

template<class C>
class list_optional
{
public:

	typedef typename C::front front_t;

	front_t const &front() const
	{
		return this->_struct.front();
	}

	typedef typename C::back back_t;

	back_t const &back() const
	{
		return this->_struct.back_value;
	}

	typedef list<typename C::pop_back> pop_back_t;

	pop_back_t pop_back() const
	{
		return this->_struct.pop_back();
	}

	typedef list<typename C::pop_front> pop_front_t;

	pop_front_t pop_front() const
	{
		return this->_struct.pop_front();
	}

	template< ::std::size_t I>
	class at_t: public C::template at_t<I> {};

	template< ::std::size_t I>
	typename at_t<I>::type const &at() const
	{
		return this->_struct.at(meta::size_t<I>());
	}

	template<class I>
	typename at_t<I::value>::type const &operator[](I) const
	{
		return this->_struct.at(meta::size_t<I::value>());
	}

	template< ::std::size_t I>
	class erase_t: public list<typename C::template erase<I>::type> {};

	template< ::std::size_t I>
	typename erase_t<I>::type erase() const
	{
		return this->_struct.erase(meta::size_t<I>());
	}

	template<class I>
	typename erase_t<I::value>::type erase(I) const
	{
		return this->_struct.erase(meta::size_t<I::value>());
	}

protected:

	list_struct<C> _struct;

};

template<>
class list_optional<meta::_detail::list_config::null>
{
protected:

	list_struct<meta::_detail::list_config::null> _struct;

};

template<class C>
class list: public list_optional<C>
{
public:

	typedef list_optional<C> optional_t;

    /// Itself (short form).
    typedef list x;

    /// Itself.
    typedef list type;

	/// Size.
	static ::std::size_t const size = C::size;

	/// Empty.
	static bool const empty = size == 0;

    /// Clear type.
    typedef list<meta::_detail::list_config::null> clear_t;

	/// Clear.
	static clear_t clear()
	{
		return clear_t(); 
	}

    /// Push front type.
    template<class T>
    class push_front_t: public list<typename C::template push_front<T>::type>
    {
    };

	/// Push front.
	template<class T>
	typename push_front_t<T>::type push_front(T const &value) const
	{
		return this->_struct.push_front(value);
	}

	/// Push back type.
	template<class T>
	class push_back_t: public list<typename C::template push_back<T>::type>
	{
	};

	/// Push back.
	template<class T>
	typename push_back_t<T>::type push_back(T const &value) const
	{
		return this->_struct.push_back(value);
	}
};

}

typedef _detail::list<meta::_detail::list_config::null>::type list;

}
}
