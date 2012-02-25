#include <iostream>

template<class T>
class ref
{
public:
	
	T &operator*() const
	{
		return *this->p;
	}

	T *operator->() const
	{
		return this->p;
	}

private:
	T *p;
};

class A
{
public:

	static void F(ref<A> const &this_)
	{
		this_->X();
	}

	void X()
	{
	}

};

int main()
{
}