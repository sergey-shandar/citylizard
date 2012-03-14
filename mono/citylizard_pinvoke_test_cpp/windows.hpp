#pragma once

#include <Windows.h>
#include <utility>

#pragma warning(push)
// C++ exception specification ignored except to indicate a function is not __declspec(nothrow)
#pragma warning(disable: 4290)

namespace windows
{

/// HRESULT
///
/// @see http://msdn.microsoft.com/en-us/library/windows/desktop/aa378137(v=vs.85).aspx
class hresult
{
public:

    hresult() throw(): _value()
    {
    }

    explicit hresult(::HRESULT value) throw(): _value(value)
    {
    }

    static void throw_if_failed(::HRESULT value) throw(hresult)
    {
        // http://msdn.microsoft.com/en-us/library/windows/desktop/ms693474(v=vs.85).aspx
        if(FAILED(value))
        {
            throw hresult(value);
        }
    }

    static ::HRESULT handle() throw()
    {
        try
        {
            throw;
        }
        catch(hresult v)
        {
            return v._value;
        }
        catch(...)
        {
            return E_FAIL;
        }
    }

private:
    ::HRESULT _value;
};

static_assert(sizeof(hresult) == sizeof(::HRESULT), "invalid hresult layout");

/// SAFEARRAY
///
/// @see http://msdn.microsoft.com/en-us/library/windows/desktop/ms221482(v=vs.85).aspx
class safearray_ptr
{
public:
    
    safearray_ptr() throw(): _p()
    {    
    }
    
    safearray_ptr(safearray_ptr const &x) throw(): _p(x._p)
    {
        this->_lock();
    }
    
    safearray_ptr(safearray_ptr &&x) throw(): _p(x._p)
    {
        x._p = 0;
    }
    
    ~safearray_ptr() throw()
    {
        this->_unlock();
    }
    
    safearray_ptr &operator=(safearray_ptr const &x) throw()
    {
        x._lock();
        this->_unlock();
        this->_p = x._p;
        return *this;
    }

    safearray_ptr &operator=(safearray_ptr &&x) throw()
    {
        ::std::swap(this->_p, x._p);
        return *this;
    }

private:
    
    ::SAFEARRAY *_p;
    
    void _lock() const throw()
    {
        if(this->_p)
        {
            // ::SafeArrayLock is not thread safe.
            // See
            // http://msdn.microsoft.com/en-us/library/windows/desktop/ms221492(v=vs.85).aspx
            ::InterlockedIncrement(&this->_p->cLocks);
        }
    }

    void _unlock() const throw()
    {
        if(this->_p)
        {
            // ::SafeArrayUnlock is not thread safe.
            // See 
            // http://msdn.microsoft.com/en-us/library/windows/desktop/ms221482(v=vs.85).aspx
            if(::InterlockedDecrement(&this->_p->cLocks) == 0)
            {
                // See
                // http://msdn.microsoft.com/en-us/library/windows/desktop/ms221702(v=vs.85).aspx
                ::SafeArrayDestroy(this->_p);
            }
        }
    }

};

static_assert(sizeof(safearray_ptr) == sizeof(::SAFEARRAY *), "invalid safearray pointer layout");

/// BSTR
///
/// @see http://msdn.microsoft.com/en-us/library/windows/desktop/ms221069(v=vs.85).aspx
class bstr
{
public:
    
    bstr() throw(): _p()
    {
    }

    bstr(bstr const &x) throw(hresult): 
        _p(x._p ? _copy_lpcolestr(x._p, x.Len()): nullptr)
    {        
    }

    bstr(bstr &&x) throw(): 
        _p(x._p)
    {
        x._p = nullptr;
    }

    template< ::UINT len>
    explicit bstr(::OLECHAR const (&x)[len]) throw(hresult): 
        _p(_copy_lpcolestr(x, len))
    {
    }

    bstr &operator=(bstr const &x) throw(hresult)
    {
        if(x._p)
        {
            this->_assign_lpcolestr(x._p, x.Len());
        }
        else
        {
            this->_free();
            this->_p = 0;
        }
        return *this;
    }

    template< ::UINT len>
    bstr &operator=(::OLECHAR const (&x)[len]) throw(hresult)
    {
        this->_assign_lpcolestr(x, len);
        return *this;
    }

    bstr &operator=(bstr &&x) throw()
    {
        ::std::swap(this->_p, x._p);
        return *this;
    }

    ~bstr() throw()
    {
        this->_free();
    }

    ::UINT Len() const throw()
    {
        return ::SysStringLen(this->_p);
    }

private:

    ::BSTR _p;

    void _free() throw()
    {
        ::SysFreeString(this->_p);
    }

    void _assign_lpcolestr(::LPCOLESTR p, ::UINT len) throw(hresult)
    {
        if(!::SysReAllocStringLen(&this->_p, p, len))
        {
            throw hresult(E_OUTOFMEMORY);
        }
    }

    static ::BSTR _copy_lpcolestr(::LPCOLESTR p, ::UINT len) throw(hresult)
    {
        ::BSTR const result = ::SysAllocStringLen(p, len);
        if(!result)
        {
            throw hresult(E_OUTOFMEMORY);
        }
        return result;
    }

};

static_assert(sizeof(bstr) == sizeof(::BSTR), "invalid bstr layout");

class variant
{
public:

    variant() throw()
    {
        this->_init();
    }

    variant(variant const &x) throw(hresult)
    {
        this->_init();
        _copy(x);
    }

    variant(variant &&x) throw()
    {
        this->_init();
        this->swap(x);
    }

    ~variant() throw()
    {
        ::VariantClear(&this->_value);
    }

    void swap(variant &x) throw()
    {
        ::std::swap(this->_value, x._value);
    }

    variant &operator=(variant const &x) throw(hresult)
    {
        this->_copy(x);
        return *this;
    }

    variant &operator=(variant &&x) throw()
    {
        this->swap(x);
        return *this;
    }

private:
    
    ::VARIANT _value;

    void _init() throw()
    {
        ::VariantInit(&this->_value);
    }

    void _copy(variant const &x) throw(hresult)
    {
        hresult::throw_if_failed(::VariantCopy(&this->_value, &x._value));
    }
};

}

#pragma warning(pop)
