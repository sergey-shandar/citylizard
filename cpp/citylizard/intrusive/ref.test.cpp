#include <citylizard/intrusive/ref.hpp>

#include <citylizard/meta/if.hpp>
#include <citylizard/cast/explicit.hpp>
#include <citylizard/intrusive/ptr.hpp>

#include <boost/test/unit_test.hpp>

namespace citylizard
{
namespace intrusive
{
namespace test
{

class ref_test_a
{
public:
    int k;
    virtual ~ref_test_a() {}
};

class ref_test_b: public ref_test_a
{
public:
    int m;
};

class ref_test_c: public ref_test_a
{
public:
    virtual void x() {}
};

}

namespace user
{

template<class T>
class traits<
    T, typename meta::if_< std::is_base_of<test::ref_test_a, T> >::type>
{
public:
    static void add(T &) throw()
    {
    }
    static void release(T &) throw()
    {
    }
    class bad_ref
    {
    };
};

}

namespace test
{

BOOST_AUTO_TEST_CASE(ref_test)
{
    // has to have element_type.
    typedef ref<ref_test_a>::element_type h;
    //
    ref_test_a a;
    // no default constructor.
    ref<ref_test_a> r0 = citylizard::cast::explicit_::value(wrap(&a));
    // copy constructor.
    ref<ref_test_a> r = r0;
    // operator=.
    r = r0;
    // swap.
    r.swap(r0);
    // operator==.
    r == r0;
    // operator!=.
    r != r0;
    // operator*.
    h &x = *r;
    x;
    // operator->.
    r->k = 0;
    // down_cast.
    /*
    ref<ref_test_b> *pb = 0;
    r = pb->down_cast<ref_test_a>();
    */
    ref_test_c c;
    ref<ref_test_c> r1(wrap(&c));
    r = citylizard::cast::explicit_::value(r1);
    r = citylizard::cast::explicit_::value(wrap(&c));
}

}
}
}

#include <citylizard/intrusive/ptr.hpp>

namespace citylizard
{
namespace intrusive
{
namespace test
{

void intrusive_bad_ref_throw(ref_test_a *)
{
    throw 0;
}

BOOST_AUTO_TEST_CASE(ref_test_ptr)
{
    // construct from ptr.
    ref<ref_test_a> r= citylizard::cast::explicit_::value(ptr<ref_test_a>());
    // assign from ptr.
    r = citylizard::cast::explicit_::value(ptr<ref_test_a>());
    // swap with ptr.
    ptr<ref_test_a> x;
    // r.swap(x);
    // operator== with ptr.
    r == x;
    // operator!= with ptr.
    r != x;
}

}
}
}

// #include <citylizard.com/intrusive/rv_ptr.hpp>

namespace citylizard
{
namespace intrusive
{
namespace test
{

BOOST_AUTO_TEST_CASE(ref_test_rv_ptr)
{
    // construct from ptr.
    ref<ref_test_a> r = citylizard::cast::explicit_::value(ptr<ref_test_a>());
    // assign from ptr.
    r = citylizard::cast::explicit_::value(ptr<ref_test_a>());
    // swap with ptr.
    // r.swap(ptr<ref_test_a>());
    // operator== with ptr.
    r == ptr<ref_test_a>();
    // operator!= with ptr.
    r != ptr<ref_test_a>();
}

}
}
}
