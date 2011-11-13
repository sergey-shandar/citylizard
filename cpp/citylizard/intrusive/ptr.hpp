#pragma once

#include <citylizard/config/_detail/config.hpp>
#include <citylizard/intrusive/_detail/holder.hpp>
#include <citylizard/intrusive/ref_fwd.hpp>
#include <citylizard/cast/safe_reinterpret.hpp>

namespace citylizard
{
/// \brief Intrusive pointers.
///
/// Advantages:
/// - Header-only library.
/// - The r-value pointers.
/// - The reference class ref which is never null.
/// - All public classes contain only one private member \c _detail and
///   All private classes and functions are in \c _detail namespace.
///   It makes IntelliSense more usefull.
/// - Fast \ref wrap / ptr::unwrap functions for raw access.
/// - \ref user::traits::bad_ref "user::traits<T>::bad_ref" can be a
///   user-defined exception. For example, a COM specific exception with \c
///   E_POINTER error code.
namespace intrusive
{

/// \brief Intrusive pointer.
///
/// \pre \ref user::traits<T> should be defined.
///
/// \sa ref, rv_ptr.
template<class T>
class ptr
{
public:

    /// Provides the type of the template parameter T.
    typedef T element_type;

#if 1

    ptr()
        throw()
    {
    }

    ptr(ptr const &b)
        throw():
        _detail(b._detail)
    {
        this->_detail.ptr_add();
    }

    ptr(ptr &&b)
        throw():
        _detail(b._detail)
    {
        b._detail.p = nullptr;
    }

    /// From a reference.
    explicit ptr(ref<T> const &b)
        throw():
        _detail(b._detail)
    {
        this->_detail.ref_add();
    }

    /// Up cast.
    template<class Derived>
    explicit ptr(ptr<Derived> const &d)
        throw():
        _detail(d._detail)
    {
        this->_detail.ptr_add();
    }

    /// Up cast.
    template<class Derived>
    explicit ptr(ptr<Derived> &&d)
        throw():
        _detail(d._detail)
    {
        d._detail.p = nullptr;
    }

    /// Up cast.
    template<class Derived>
    explicit ptr(ref<Derived> const &d)
        throw():
        _detail(d._detail)
    {
        this->_detail.ref_add();
    }

#endif

    ~ptr()
        throw()
    {
        this->_detail.ptr_release();
    }

    void reset()
        throw()
    {
        this->_detail.ptr_reset();
    }

#if 1

    ptr &operator=(ptr const &b)
        throw()
    {
        b._detail.ptr_add();
        this->_detail.ptr_release();
        this->_detail = b._detail;
        return *this;
    }
    ptr &operator=(ptr &&b)
        throw()
    {
        this->swap(b);
        return *this;
    }

    /*
    ptr &operator=(ref<T> const &b)
        throw()
    {
        b._detail.ref_add();
        this->_detail.ptr_release();
        this->_detail = b._detail;
        return *this;
    }
    */

#endif

#if 1

    void swap(ptr &b)
        throw()
    {
        this->_detail.swap(b._detail);
    }

    /*
    /// \warning the function throws user::traits<T>::bad_ref() if the pointer is
    /// null.
    void swap(ref<T> &b)
        throw(...)
    {
        b.swap(*this);
    }
    */

#endif

#if 1

    bool operator==(ptr const &b) const
        throw()
    {
        return this->_detail.is_equal(b._detail);
    }
    bool operator!=(ptr const &b) const
        throw()
    {
        return !this->_detail.is_equal(b._detail);
    }
    bool operator==(ref<T> const &b) const
        throw()
    {
        return this->_detail.is_equal(b._detail);
    }
    bool operator!=(ref<T> const &b) const
        throw()
    {
        return !this->_detail.is_equal(b._detail);
    }

#endif

#if 1

    /// \warning the function throws user::traits<T>::bad_ref() if the pointer is
    /// null.
    T &operator*() const
        throw(decltype(user::traits<T>::bad_ref()))
    {
        return *this->_detail.ref_check().p;
    }

    /// \warning the function throws user::traits<T>::bad_ref() if the pointer is
    /// null.
    T *operator->() const
        throw(decltype(user::traits<T>::bad_ref()))
    {
        return this->_detail.ref_check().p;
    }

#endif

    // casting
#if 1

    // explicit operator bool() const
    bool bool_cast() const
    {
        return this->_detail.bool_cast();
    }

    /// Raw access.
    /// \sa wrap.
    T *const &unwrap() const
        throw()
    {
        return cast::safe_reinterpret::ref(*this);
    }

    /// Raw access.
    /// \sa wrap.
    T * &unwrap()
        throw()
    {
        return cast::safe_reinterpret::ref(*this);
    }

#endif

private:
    _detail::holder<T> _detail;
    template<class Base> friend class ref;
    template<class Base> friend class ptr;
};

/// \sa ptr::unwrap.
template<class T>
ptr<T> const &wrap(T *const &p)
    throw()
{
    return cast::safe_reinterpret::ref(p);
}

/// \sa ptr::unwrap.
template<class T>
ptr<T> &wrap(T * &p)
    throw()
{
    return cast::safe_reinterpret::ref(p);
}

}
}