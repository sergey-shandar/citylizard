#pragma once

#include <citylizard/meta/list.hpp>

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
};

template<class Back>
class list_struct<meta::_detail::list_config::one<Back> >
{
public:

	Back back_value;

	const Back &front() const
	{
		return this->back_value;
	}


};

template<class PopBack, class B>
class list_struct<meta::_detail::list_config::append<PopBack, Back> >
{
private:

	list<PopBack> pop_back_value;

public:

	Back back_value;

	const PopBack::front &front() const
	{
		return this->pop_back_value.front(); 
	}

};

template<class C>
class list_optional
{
public:

	typedef typename C::front front_t;

	const front_t &front() const
	{
		return this->_struct.front();
	}

	typedef typename C::back back_t;

	const back_t &back() const
	{
		return this->_struct.back_value;
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

	typedef list type;

};

}

typedef _detail::list<meta::list>::type list;

}
}