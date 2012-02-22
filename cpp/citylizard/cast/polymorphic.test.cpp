#include <citylizard/cast/polymorphic.hpp>

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

BOOST_AUTO_TEST_CASE(cast_polymorphic_test)
{
    using namespace citylizard::cast;
    //
    b bx;
    a& ax = polymorphic::ref(bx);
    b *by2 = polymorphic::value(&ax);
    b &by2r = polymorphic::ref(ax);
}