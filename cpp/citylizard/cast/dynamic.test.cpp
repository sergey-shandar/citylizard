#include <citylizard/cast/dynamic.hpp>

#include <boost/test/unit_test.hpp>

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

BOOST_AUTO_TEST_CASE(cast_dynamic_test)
{
    using namespace citylizard::cast;

    b bx;
    a& ax = dynamic::ref(bx);
    b * pbx = &bx;
    a *pax = dynamic::value(pbx);
    pax;
    b& by = dynamic::ref(ax);
    by;
    a *pax1 = 0;
    b *by1 = dynamic::value(pax1);
}