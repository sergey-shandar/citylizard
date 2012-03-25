#include <Windows.h>

#include <iostream>

template<class T>
class atomic_ptr
{
public:

    atomic_ptr() throw(): _value() {}

    explicit atomic_ptr(T *const value) throw(): _value(value) {}

    T *const get() throw() 
    { 
        return this->_interlock_compare_exchange(nullptr, nullptr); 
    }

    void set(T *const value) throw()
    {
        InterlockedExchangePointer(this->_destination(), value);
    }

    bool set_if(T *const value, T *const comparand) throw()
    {
        return this->_interlock_compare_exchange(value, comparand) == comparand;
    }

private:
    
    T *_value;

    atomic_ptr(atomic_ptr const &) throw();
    atomic_ptr &operator=(atomic_ptr const &) throw();

    T *const _interlock_compare_exchange(T *const exchange, T *const comparand)
        throw()
    {
        return static_cast<T *const>(::InterlockedCompareExchangePointer(
            this->_destination(), exchange, comparand));
    }

    void * *const _destination() throw() 
    {
        return reinterpret_cast<void * *const>(&this->_value); 
    }

};

class node
{
public:

private:

    atomic_ptr<node> _next;

    friend class stack;
};

/// @note this stack suffers from ABA problem
/// http://en.wikipedia.org/wiki/ABA_problem
class stack
{
public:

    void push_front(node *new_first)
    {
        for(;;)
        {
            node *const old_first = this->_first.get();
            new_first->_next.set(old_first);
            if(this->_first.set_if(new_first, old_first))
            {
                break;
            }
        }

    }

    node *pop_front()
    {
        for(;;)
        {
            node *const old_first = this->_first.get();
            node *const new_first = old_first->_next.get();
            if(this->_first.set_if(new_first, old_first))
            {
                break;
            }
        }
    }

private:

    atomic_ptr<node> _first;

};

int main()
{
}
