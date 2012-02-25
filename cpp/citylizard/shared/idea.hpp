#include <citylizard/intrusive/ptr.hpp>

namespace citylizard
{
namespace shared
{

/*
template<class T>
class ptr
{
public:



private:

	class _wrap_t
	{
	public:
		T value;
	};

	intrusive::ptr<_wrap_t> _detail;

};
*/

/// C++ intrusive dynamic model (dynamic_cast) 1 pointer.
/// COM model (everything is derived from interface) 1 pointer
/// C++ wrap model, 2 pointers, one on data, one one wrap.

template<class T>
class policy
{
public:

	typedef T value_t

	static T &make();

	template<class C>
	static T &make(C const &);

	static void add_ref(T &);

	static void release(T &);

};

template<class T>
class com_class_ref
{
public:

	T &operator*() const throw();

	T *operator->() const throw();

};

class A 
{
	static void F(com_class_ref<A> const &this_)
	{
	}
};

template<class T>
class com_policy
{
public:

	typedef T value_t

	static T &make()
	{
		T &r = *new T();
	}

	template<class C>
	static T &make(C const &);

	static void add_ref(T &);

	static void release(T &);

};

template<class P>
class ptr
{
public:

	typedef typename P::value_t value_t;

	value_t &operator*() const
	{
		return this->_p.get();
	}

private:

	P _p;

};

class shared
{
private:
	virtual void add_ref() = 0;
	virtual void release() = 0;
};

}
}
