#include <citylizard/cast/polymorphic_down.hpp>

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

BOOST_AUTO_TEST_CASE(polymorphic_down_test)
{
    using namespace citylizard::cast;
    //
    b bx;
    a& ax = polymorphic_down::ref(bx);
    b *by2 = polymorphic_down::value(&ax);
    b &by2r = polymorphic_down::ref(ax);
}