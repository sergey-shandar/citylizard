#include <iostream>
#include <vector>

class shared
{
private:
	virtual void _detail_shared_add_ref() const throw() = 0;
	virtual bool _detail_shared_release() const throw() = 0;
	template<class T> friend class ref;
};

namespace _detail
{

class counter
{
protected:
	
	counter() throw(): _detail_counter(1)
	{
	}

	::std::size_t _detail_counter;

private:

	counter(counter const &);
	counter &operator=(counter const &);
};

template<class T>
class shared_wrap: private counter, public T
{
public:

	shared_wrap(): T()
	{
	}

	template<class P>
	shared_wrap(P &&p): T(::std::forward(p))
	{
	}

private:
	
	void _detail_shared_add_ref() const throw() override 
	{
		++this->_detail_counter;
	}

	bool _detail_shared_release() const throw() override 
	{
		--this->_detail_counter;
		return this->_detail_counter == 0;
	}

};

}

template<class T>
class ref
{
public:

	explicit ref(T &b) throw(): ref(&b)
	{
		this->_detail_p->_detail_shared_add_ref();
	}

	ref(ref const &b) throw(): ref(*b.p)
	{
	}

	ref &operator=(ref const &b) throw()
	{
		b._detail_p->_detail_shared_add_ref();
		this->_detail_release();
		this->_detail_p = b;
	}

	~ref() throw()
	{
		this->_detail_release();
	}

	static ref make()
	{
		return ref(new _detail::shared_wrap<T>());
	}

	template<class P>
	static ref make(P &&p)
	{
		return ref(new _detail::shared_wrap<T>(::std::forward(p)));
	}

private:

	explicit ref(T *b): _detail_p(b)
	{
	}

	T *_detail_p;

	void _detail_release()
	{
		if(this->_detail_p->_detail_shared_release())
		{
			delete this->_detail_p;
		}
	}
};

class my: public shared
{
public:

	void A() {}

};

int main()
{
	auto p = ref<my>::make();
}

// first: COM interfaces
//	- casting:
//		- upcasting to COM interfaces.
//		- to other COM interfaces by QueryInterface.
// second: C++ objects derived from 'shared'
//	- casting:
//		- up/dynamic to anything derived form 'shared'.
// third: C++ objects not derived from shared.
//	- with casting:
//		- requires two pointers
//	- no cast:
//		- one pointer.