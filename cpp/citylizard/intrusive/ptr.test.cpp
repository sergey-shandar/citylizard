#include <citylizard/intrusive/ptr.hpp>
#include <citylizard/meta/if.hpp>

#include <boost/test/unit_test.hpp>

namespace citylizard_com
{
namespace intrusive
{
namespace test
{

class ptr_test_a
{
public:
    int counter;
    int d;
    ptr_test_a(): 
        counter(0)
    {
    }
};

class ptr_test_b: public ptr_test_a
{
public:
};

}

namespace user
{

template<class T>
class traits<
    T, typename meta::if_< ::std::is_base_of<test::ptr_test_a, T> >::type>
{
public:
    static void add(T &x) throw()
    {
        ++x.counter;
    }
    static void release(T &x) throw()
    {
        --x.counter;
    }
    class bad_ref
    {
    };
};

}
}
}

BOOST_AUTO_TEST_CASE(ptr_test)
{
    namespace I = ::citylizard_com::intrusive;
    namespace T = ::citylizard_com::intrusive::test;
    //
    T::ptr_test_a object;
    T::ptr_test_b object_b;
    {
        // has to have element_type.
        typedef I::ptr<T::ptr_test_a>::element_type h;
        // default constructor.
        I::ptr<T::ptr_test_a> a;
        BOOST_CHECK(a.unwrap() == 0);
        I::ptr<T::ptr_test_a> fa;
        BOOST_CHECK(fa.unwrap() == 0);
        // copy constructor.
        I::ptr<T::ptr_test_a> a0 = a;
        BOOST_CHECK(a0.unwrap() == 0);
        I::ptr<T::ptr_test_a> fa0 = I::wrap(&object);
        BOOST_CHECK(fa0.unwrap() == &object);
        BOOST_CHECK(object.counter == 1);
        // operator=.
        a0 = a;
        BOOST_CHECK(a0.unwrap() == 0);
        fa = fa0;
        BOOST_CHECK(fa0.unwrap() == fa.unwrap());
        BOOST_CHECK(object.counter == 2);
        // reset.
        a0.reset();
        BOOST_CHECK(a0.unwrap() == 0);
        fa0.reset();
        BOOST_CHECK(fa0.unwrap() == 0);
        BOOST_CHECK(object.counter == 1);
        // swap.
        a0.swap(a);
        BOOST_CHECK(a0.unwrap() == 0);
        BOOST_CHECK(a.unwrap() == 0);
        fa.swap(fa0);
        BOOST_CHECK(fa.unwrap() == 0);
        BOOST_CHECK(fa0.unwrap() == &object);
        BOOST_CHECK(object.counter == 1);
        // operator==.
        BOOST_CHECK(a0 == a);
        BOOST_CHECK(!(fa == fa0));
        // operator!=.
        BOOST_CHECK(!(a0 != a));
        BOOST_CHECK(fa != fa0);
        // operator*.
        try
        {
            T::ptr_test_a &ra = *a0;
            BOOST_CHECK(false);
            ra;
        }
        catch(I::user::traits<T::ptr_test_a>::bad_ref const &)
        {
            BOOST_CHECK(true);
        }
        catch(...)
        {
            BOOST_CHECK(false);
        }
        try
        {
            T::ptr_test_a &ra = *fa0;
            ra.d = 5;
            BOOST_CHECK(&ra == &object);
            BOOST_CHECK(ra.counter == 1);
        }
        catch(...)
        {
            BOOST_CHECK(false);
        }
        // operator->
        try
        {
            a0->d = 5;
        }
        catch(I::user::traits<T::ptr_test_a>::bad_ref const &)
        {
            BOOST_CHECK(true);
        }
        catch(...)
        {
            BOOST_CHECK(false);
        }
        try
        {
            fa0->d = 5;
        }
        catch(...)
        {
            BOOST_CHECK(false);
        }

        I::ptr<T::ptr_test_b> b;

        // operator bool
        BOOST_CHECK(!a0.bool_cast());
        bool const m = a0.bool_cast();
        BOOST_CHECK(!m);
        BOOST_CHECK(a0.bool_cast() ? false: true);
        if(a0.bool_cast() || a.bool_cast())
        {
            BOOST_CHECK(false);
        }
        BOOST_CHECK(fa0.bool_cast());

        /*
        // operator bool
        BOOST_CHECK(!static_cast<bool>(a0));
        // BOOST_CHECK(!a0.bool_cast());
        BOOST_CHECK(!(bool)a0);
        BOOST_CHECK(!a0);
        bool const m = bool(a0);
        // bool const m = a0.bool_cast();
        BOOST_CHECK(!m);
        BOOST_CHECK(a0 ? false: true);
        if(a0 || a)
        {
            BOOST_CHECK(false);
        }
        BOOST_CHECK(static_cast<bool>(fa0));
        // BOOST_CHECK(fa0.bool_cast());
        BOOST_CHECK((bool)fa0);
        if(fa0)
        {
        }
        else
        {
            BOOST_CHECK(false);
        }
        // must not be compiled
        BOOST_CHECK(a == b);
        */

        // down_cast
        I::ptr<T::ptr_test_a> const ab = b.down_cast<T::ptr_test_a>();
        BOOST_CHECK(ab.unwrap() == 0);
        I::ptr<T::ptr_test_b> fb = I::wrap(&object_b);
        BOOST_CHECK(object_b.counter == 1);
        I::ptr<T::ptr_test_a> fab = fb.down_cast<T::ptr_test_a>();
        BOOST_CHECK(object_b.counter == 2);
        BOOST_CHECK(fab.unwrap() == &static_cast<T::ptr_test_a &>(object_b));
        // unwrap
        fa0.unwrap()->d = 7;
        {
            T::ptr_test_b * &pab = fb.unwrap();
            if(pab)
            {
                I::user::traits<T::ptr_test_b>::release(*pab);
            }
            pab = 0;
            BOOST_CHECK(object_b.counter == 1);
            BOOST_CHECK(!fb.bool_cast());
        }
        // wrap
        {
            T::ptr_test_a *pa = 0;
            I::wrap(pa).swap(fab);
            if(pa)
            {
                I::user::traits<T::ptr_test_a>::release(*pa);
            }
            pa = 0;
            BOOST_CHECK(object_b.counter == 0);
            I::wrap(&object)->d = 90;
            BOOST_CHECK(object.d == 90);
        }
    }
    BOOST_CHECK(object.counter == 0);
    BOOST_CHECK(object_b.counter == 0);
}

#include <citylizard/intrusive/ref.hpp>

BOOST_AUTO_TEST_CASE(ptr_test_ref)
{    
    namespace I = ::citylizard_com::intrusive;
    namespace T = ::citylizard_com::intrusive::test;
    T::ptr_test_a object;
    {
        I::ref<T::ptr_test_a> ra = I::wrap(&object);
        BOOST_CHECK(object.counter == 1);
        // constructor from ref.
        I::ptr<T::ptr_test_a> a = ra;
        BOOST_CHECK(object.counter == 2);
        a.reset();
        BOOST_CHECK(object.counter == 1);
        // assignment from ref.
        a = ra;
        BOOST_CHECK(object.counter == 2);
        // swap with ref.
        a.swap(ra);
        BOOST_CHECK(object.counter == 2);
        try
        {
            I::ptr<T::ptr_test_a> an;
            an.swap(ra);
            BOOST_CHECK(false);
        }
        catch(I::user::traits<T::ptr_test_a>::bad_ref const &)
        {
        }
        catch(...)
        {
            BOOST_CHECK(false);
        }
        // operator== with ref.
        BOOST_CHECK(a == ra);
        BOOST_CHECK(!(I::ptr<T::ptr_test_a>() == ra));
        // operator!= with ref.
        BOOST_CHECK(!(a != ra));
        BOOST_CHECK(I::ptr<T::ptr_test_a>() != ra);
    }
    BOOST_CHECK(object.counter == 0);
}

// #include <citylizard.com/intrusive/rv_ptr.hpp>

BOOST_AUTO_TEST_CASE(ptr_test_rv_ptr)
{
    namespace I = ::citylizard_com::intrusive;
    namespace T = ::citylizard_com::intrusive::test;
    T::ptr_test_a object;
    {
        // Constructor from ptr.
        I::ptr<T::ptr_test_a> a = I::ptr<T::ptr_test_a>();
        I::ptr<T::ptr_test_a> ra = I::wrap(&object);
        BOOST_CHECK(object.counter == 1);
        I::ptr<T::ptr_test_a> oa = static_cast<I::ptr<T::ptr_test_a> &&>(ra);
        BOOST_CHECK(object.counter == 1);
        // Assignment from ptr.
        oa = I::ptr<T::ptr_test_a>();
        BOOST_CHECK(oa.unwrap() == 0);
        BOOST_CHECK(object.counter == 0);
        // swap with ptr.
        auto a2 = I::ptr<T::ptr_test_a>();
        a.swap(a2);
        BOOST_CHECK(a.unwrap() == 0);
        ra = I::wrap(&object);
        BOOST_CHECK(object.counter == 1);
        oa.swap(ra);
        BOOST_CHECK(object.counter == 1);
        // operator== with ptr.
        BOOST_CHECK(a == I::ptr<T::ptr_test_a>());
        BOOST_CHECK(!(oa == I::ptr<T::ptr_test_a>()));
        // operator!= with ptr.
        BOOST_CHECK(!(a != I::ptr<T::ptr_test_a>()));
        BOOST_CHECK(oa != I::ptr<T::ptr_test_a>());
    }
    BOOST_CHECK(object.counter == 0);
}
