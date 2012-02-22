#pragma once

#include <citylizard/intrusive/_detail/holder.hpp>
#include <citylizard/intrusive/ptr_fwd.hpp>
#include <citylizard/intrusive/ref_fwd.hpp>

namespace citylizard
{
namespace intrusive
{

/// \brief Intrusive not-null reference. 
///
/// It is the same as ptr<T> but does not allow assign null.
///
/// \pre \ref user::traits<T> should be defined.
///
/// \sa ptr, rv_ptr.
template<class T>
class ref
{
public:

    /// Provides the type of the template parameter T.
    typedef T element_type;

#if 1

    ref(ref const &x)
        throw():         
        _detail(x._detail)
    {
        this->_detail.ref_add();
    }
    
    /// From a pointer.
    /// \warning the function throws \b user::traits<T>::bad_ref() if the 
    /// parameter is null.
    explicit ref(ptr<T> const &b) 
        throw(...):
        _detail(b._detail.ref_check())
    {
        this->_detail.ref_add();
    }

    /// From a pointer.
    /// \warning the function throws \b user::traits<T>::bad_ref() if the 
    /// parameter is null.
    explicit ref(ptr<T> &&b) 
        throw(...):
        _detail(b._detail.ref_check())
    {
        b._detail.p = 0;
    }

    /// Up cast.
    template<class Derived>
    explicit ref(ref<Derived> const &d)
        throw():
        _detail(d._detail)
    {
        this->_detail.ref_add();
    }

    /// Up cast.
    template<class Derived>
    explicit ref(ptr<Derived> const &d)
        throw(...):
        _detail(d._detail.ref_check())
    {
        this->_detail.ref_add();
    }

    /// Up cast.
    template<class Derived>
    explicit ref(ptr<Derived> &&d)
        throw(...):
        _detail(d._detail.ref_check())
    {
        d._detail.p = 0;
    }

#endif

    // Destructor.
#if 1
    ~ref()
        throw()
    {
        this->_detail.ref_release();
    }
#endif

    // Assignments.
#if 1

    /// Copy assignment.
    ref &operator=(ref const &b) 
        throw()
    {
        this->_detail.ref_assign(b._detail);
        return *this;
    }

    /// Copy assignment.
    ref &operator=(ref &&b)
        throw()
    {
        this->swap(b);
        return *this;
    }

    /// \warning the function throws \b user::traits<T>::bad_ref() if the 
    /// parameter is null.
    /*
    ref &operator=(ptr<T> const &b) 
        throw(...)
    {
        this->_detail.ref_assign(b._detail.ref_check());
        return *this;
    }
    */

    /// \warning the function throws \b user::traits<T>::bad_ref() if the 
    /// parameter is null.
    /*
    ref &operator=(ptr<T> &&b) 
        throw(...)
    {
        this->swap(b);
        return *this;
    }
    */

#endif

    // Swaps.
#if 1

    /// Swap.
    void swap(ref &b) 
        throw()
    {
        this->_detail.swap(b._detail);
    }

    /*
    /// \warning the function calls \b intrusive_bad_ref_throw((T *)0) if the 
    /// parameter is null.
    void swap(ptr<T> &b)
        throw(...)
    {
        b._detail.ref_check();
        this->_detail.swap(b._detail);
    }
    */

#endif

    // Equality.
#if 1

    /// Equal to the reference.
    bool operator==(ref const &b) const
        throw()
    {
        return this->_detail.is_equal(b._detail);
    }

    /// Not equal to the reference.
    bool operator!=(ref const &b) const
        throw()
    {
        return !this->_detail.is_equal(b._detail);
    }

    /// Equal to the pointer.
    bool operator==(ptr<T> const &b) const
        throw()
    {
        return this->_detail.is_equal(b._detail);
    }

    /// Not equal to the pointer.
    bool operator!=(ptr<T> const &b) const
        throw()
    {
        return !this->_detail.is_equal(b._detail);
    }

#endif

#if 1

    /// Inderiction.
    T &operator*() const
        throw()
    {
        CITYLIZARD_INTRUSIVE_DETAIL_ASSUME(this->_detail.p);
        return *this->_detail.p;
    }

    /// Member access.
    T *operator->() const
        throw()
    {
        CITYLIZARD_INTRUSIVE_DETAIL_ASSUME(this->_detail.p);
        return this->_detail.p;
    }

#endif

private:
    _detail::holder<T> _detail;
    template<class Base> friend class ptr;
    template<class Base> friend class ref;
};

}
}
