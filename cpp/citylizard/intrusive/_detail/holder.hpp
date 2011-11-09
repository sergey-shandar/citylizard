#pragma once

#include <citylizard/intrusive/_detail/assume.hpp>
#include <citylizard/intrusive/user/traits.hpp>
#include <boost/type_traits/is_base_of.hpp>
#include <algorithm>

#pragma warning(push)
// C++ exception specification ignored except to indicate a function is not 
// __declspec(nothrow)
#pragma warning(disable: 4290)

namespace citylizard
{
namespace intrusive
{

namespace _detail
{

template<class T>
class holder
{
public:

    // Constructors.
#if 1
    holder() 
        throw(): 
        p()
    {
    }

    // Up cast.
    template<class Derived>
    explicit holder(holder<Derived> const &d) 
        throw():
        p(d.p)        
    {
        static_assert(
            ::boost::is_base_of<T, Derived>::value,
            "the 'T' type is not a base class of the 'Derived' type");
    }

#endif

    typedef user::traits<T> traits;

    // Reference.
#if 1
    void ref_add() const 
        throw()
    {
        CITYLIZARD_INTRUSIVE_DETAIL_ASSUME(this->p);
        traits::add(*this->p);
    }
    void ref_release() const 
        throw()
    {
        CITYLIZARD_INTRUSIVE_DETAIL_ASSUME(this->p);
        traits::release(*this->p);
    }
    void ref_assign(holder const &b) 
        throw()
    {
        b.ref_add();
        this->ref_release();
        this->p = b.p;
    }
    holder const &ref_check() const 
        throw(decltype(traits::bad_ref()))
    {
        if(!this->p)
        {
            throw traits::bad_ref();
        }
        return *this;
    }
#endif

    // ptr
#if 1
    void ptr_add() const throw()
    {
        if(this->p)
        {
            this->ref_add();
        }
    }
    void ptr_release() const throw()
    {
        if(this->p)
        {
            this->ref_release();
        }
    }
    void ptr_reset() throw()
    {
        if(this->p)
        {
            this->ref_release();
            this->p = 0;
        }
    }

#endif

    void swap(holder &b) 
        throw()
    {
        ::std::swap(this->p, b.p);
    }

    bool is_equal(holder const &b) const 
        throw()
    {
        return this->p == b.p;
    }

    bool bool_cast() const throw()
    {
        return this->p != 0;
    }

    T *p;
};

}
}
}

#pragma warning(pop)
