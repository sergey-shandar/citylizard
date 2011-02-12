// 'operation' : conversion from 'type1' to 'type2' of greater size
// http://msdn2.microsoft.com/en-us/library/h97f4b9y.aspx
#pragma warning(disable: 4312)

#include <citylizard/cast/polymorphic.hpp>
#include <citylizard/cast/polymorphic_down.hpp>
#include <citylizard/cast/static.hpp>
#include <citylizard/cast/const.hpp>
#include <citylizard/cast/dynamic.hpp>
#include <citylizard/cast/reinterpret.hpp>
#include <citylizard/cast/safe_reinterpret.hpp>
#include <citylizard/cast/explicit.hpp>
#include <citylizard/cast/default.hpp>

#include <iostream>

#include <boost/test/unit_test.hpp>

class MyClass 
{
public:
	MyClass() {}
	explicit MyClass(char) {}
};

struct a
{
	virtual ~a()
	{
	}
			
	void f()
	{
	}
};

struct d
{
	virtual ~d()
	{
	}
	int c;
};

struct b : d, a
{
	void g()
	{
	}
};

struct throw_sfinae
{
	struct sfinae
	{
	};
	explicit throw_sfinae(int)
	{
		throw sfinae();
	}
};

BOOST_AUTO_TEST_CASE(cast_test)
{
	namespace cast = citylizard::cast;

	// MyClass my1 = '5'; // error
	MyClass my = cast::explicit_::value('5');

	MyClass my3 = cast::default_::value();

	char m = cast::static_::to<char>::apply(5); // ok
	m;
	// char const &m1 = cast::reinterpret::to<char const &>::apply(5); // error
	int const dc = 7;
	char const &m1 = cast::reinterpret::to<char const &>::apply(dc); // ok
	m1;
	int dv = 7;
	char &m2 = cast::reinterpret::to<char &>::apply(dv); // ok
	m2;
	int i1 = cast::static_::value('a');
	int const &i2 = i1;
	int &i3 = cast::const_::ref(i2);
	i3 = 2;
	b bx;
	a& ax = cast::static_::ref(bx);
	b * pbx = &bx;
	a *pax = cast::static_::value(pbx);
	pax;
	b& by = cast::dynamic::ref(ax);
	by;
	a *pax1 = 0;
	b *by1 = cast::dynamic::value(pax1);
	by1;
	b *by2 = cast::polymorphic::value(&ax);
	b &by2r = cast::polymorphic::ref(ax);
	a *ax2 = cast::polymorphic_down::value(by2);
	a &ax2r = cast::polymorphic_down::ref(by2r);
	ax2;
	::ptrdiff_t pd = 3;
	int* p = cast::safe_reinterpret::value(pd);
	p;
	int* p1 = cast::reinterpret::value(i1);
	try
	{
		d dd;
		throw_sfinae &ts = cast::dynamic::ref(dd);
	}
	catch(...)
	{
		::std::cout << "Good" << ::std::endl;
	}
}
