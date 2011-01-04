#pragma once

#include <citylizard/intrusive/_detail/holder.hpp>
#include <citylizard/intrusive/_detail/down_cast.hpp>
#include <citylizard/intrusive/ptr_fwd.hpp>
#include <citylizard/intrusive/ref_fwd.hpp>

namespace citylizard
{
namespace intrusive
{

/// \brief Intrusive reference.
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
    
    /// \warning the function throws \b user::traits<T>::bad_ref() if the 
    /// parameter is null.
    ref(ptr<T> const &b) 
        throw(...):
        _detail(b._detail.ref_check())
    {
        this->_detail.ref_add();
    }
    /// \warning the function throws \b user::traits<T>::bad_ref() if the 
    /// parameter is null.
    ref(ptr<T> &&b) 
        throw(...):
        _detail(b._detail.ref_check())
    {
        b._detail.p = 0;
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
    // Copy assignment.
    ref &operator=(ref const &b) 
        throw()
    {
        this->_detail.ref_assign(b._detail);
        return *this;
    }
    ref &operator=(ref &&b)
        throw()
    {
        this->swap(b);
        return *this;
    }
    /// \warning the function throws \b user::traits<T>::bad_ref() if the 
    /// parameter is null.
    ref &operator=(ptr<T> const &b) 
        throw(...)
    {
        this->_detail.ref_assign(b._detail.ref_check());
        return *this;
    }
    /// \warning the function throws \b user::traits<T>::bad_ref() if the 
    /// parameter is null.
    ref &operator=(ptr<T> &&b) 
        throw(...)
    {
        this->swap(b);
        return *this;
    }
#endif

    // Swaps.
#if 1
    void swap(ref &b) 
        throw()
    {
        this->_detail.swap(b._detail);
    }
    /// \warning the function calls \b intrusive_bad_ref_throw((T *)0) if the 
    /// parameter is null.
    void swap(ptr<T> &b)
        throw(...)
    {
        b._detail.ref_check();
        this->_detail.swap(b._detail);
    }

#endif

    // Equality.
#if 1
    bool operator==(ref const &b) const
        throw()
    {
        return this->_detail.is_equal(b._detail);
    }
    bool operator!=(ref const &b) const
        throw()
    {
        return !this->_detail.is_equal(b._detail);
    }
    bool operator==(ptr<T> const &b) const
        throw()
    {
        return this->_detail.is_equal(b._detail);
    }
    bool operator!=(ptr<T> const &b) const
        throw()
    {
        return !this->_detail.is_equal(b._detail);
    }

#endif

#if 1
    T &operator*() const
        throw()
    {
        CITYLIZARD_INTRUSIVE_DETAIL_ASSUME(this->_detail.p);
        return *this->_detail.p;
    }
    T *operator->() const
        throw()
    {
        CITYLIZARD_INTRUSIVE_DETAIL_ASSUME(this->_detail.p);
        return this->_detail.p;
    }
#endif

#if 1
    template<class B>
    ref<B> const &down_cast() const
        throw()
    {
        return _detail::down_cast<ref<B> >(*this);
    }
#endif

private:
    _detail::holder<T> _detail;
    friend class ptr<T>;
};

}
}
