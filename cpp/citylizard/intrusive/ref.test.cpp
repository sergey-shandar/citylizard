#include <citylizard/intrusive/ref.hpp>

#include <citylizard/meta/if.hpp>

namespace citylizard_com
{
namespace intrusive
{
namespace test
{

class ref_test_a
{
public:
    int k;
};

class ref_test_b: public ref_test_a
{
public:
    int m;
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

void ref_test()
{
    // has to have element_type.
    typedef ref<ref_test_a>::element_type h;
    // no default constructor.
    ref<ref_test_a> *pr = 0;
    // copy constructor.
    ref<ref_test_a> r = *pr;
    // operator=.
    r = *pr;
    // swap.
    r.swap(*pr);
    // operator==.
    r == *pr;
    // operator!=.
    r != *pr;
    // operator*.
    h &x = *r;
    x;
    // operator->.
    r->k = 0;
    // down_cast.
    ref<ref_test_b> *pb = 0;
    r =    pb->down_cast<ref_test_a>();
}

}
}
}

#include <citylizard/intrusive/ptr.hpp>

namespace citylizard_com
{
namespace intrusive
{
namespace test
{

void intrusive_bad_ref_throw(ref_test_a *)
{
    throw 0;
}

void ref_test_ptr()
{
    // construct from ptr.
    ref<ref_test_a> r = ptr<ref_test_a>();
    // assign from ptr.
    r = ptr<ref_test_a>();
    // swap with ptr.
    ptr<ref_test_a> x;
    r.swap(x);
    // operator== with ptr.
    r == x;
    // operator!= with ptr.
    r != x;
}

}
}
}

// #include <citylizard.com/intrusive/rv_ptr.hpp>

namespace citylizard_com
{
namespace intrusive
{
namespace test
{

void ref_test_rv_ptr()
{
    // construct from ptr.
    ref<ref_test_a> r = ptr<ref_test_a>();
    // assign from ptr.
    r = ptr<ref_test_a>();
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
