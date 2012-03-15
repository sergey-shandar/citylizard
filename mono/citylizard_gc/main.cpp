#include <iostream>

class as_prior { };

class as_next { };

template<class T>
class node
{
public:

    node() throw()
    {
        this->_prior = this->_next = &this->_this();
    }

    node(T &list, as_prior): _prior(list._prior), _next(&list)
    {
        this->_norm();
    }

    node(T &list, as_next): _prior(&list), _next(list._next)
    {
        this->_norm();
    }

    void disconnect() throw()
    {
        this->_disconnect();
        this->_prior = this->_next = &this->_this();
    }

    void set_prior(T &prior) throw()
    {
        prior._reconnect(*this->_prior, this->_this());
    }

    void set_next(T &next) throw()
    {
        next._reconnect(this->_this(), *this->_next);
    }

    T &get_prior() const throw()
    {
        return *this->_prior;
    }

    T &get_next() const throw()
    {
        return *this->_next;
    }

    bool empty() const throw()
    {
        return this->_prior == this->_next;
    }

private:

    T *_prior;
    T *_next;

    /// disconnect from old list.
    void _disconnect() throw()
    {
        this->_prior->_next = this->_next;
        this->_next->_prior = this->_prior;
    }

    void _reconnect(T &before, T &after) throw()
    {
        // disconnect from old list.
        this->_disconnect();
        // connect to new list
        this->_prior = &before;
        this->_next = &after;
        this->_norm();
    }

    void _norm() throw()
    {
        this->_prior->_next = this->_next->_prior = &this->_this();
    }

    T &_this() throw()
    {
        return *static_cast<T *>(this);
    }

    node(node const &);
    node &operator=(node const &);
};

/*
const int list_root     = 0x0;
const int list_a        = 0x2;
const int list_b        = 0x3;
const int list_root     = 0x4;

class node
{
public:
    node *prior;
    node *next;
    int   list_id;
    virtual ~node() = 0;
    virtual node *get_child(int i) = 0;

    node(int list_id_): prior(this), next(this), list_id(list_id)
    {
    }

    explicit node(node &before): prior(&before), next(before.next), list_id(before.list_id)
    {
        before.next = this->next->prior = this;
    }

    void move(node &before)
    {
        // disconnect from old list.
        this->prior->next = this->next;
        this->next->prior = this->prior;
        // connect to new list.
        this->prior = &before;
        this->next = before.next;
        before.next = this->next->prior = this;
        //
        list_id = before.list_id;
    }
};

Node list[4];
*/

// 1. current_list = list_a | list_b, empty_list = list_b | list_a
// 2. scan root_list and add all children to list_to_check
// 3. scan list_to_check and add all children to list_to_check, after checking
//    children the particular node, add this node to empty_list.
// 4. kill all nodes in current_list.
// 5. swap current_list and empty_list definitions.
// 6. goto 2.

// when object is created, it is added to to_check list.
// if object A start referencing to object B:
//  if A is in empty_list and B is in current_list then add B to list_to_check.

class my: public node<my>
{
public:

    my() throw(): _id() {}

    template<class T>
    my(my &x, T tag) throw(): node<my>(x, tag), _id(x._id) {}

    virtual ~my() {}

    virtual my *get(int i) const throw() { return nullptr; }

    int get_id() const throw() { return this->_id; }

private:

    int _id;

};

int main(int argc, char **argv)
{
    my x;

    for(int i = 0; i < 10000; ++i)
    {
        new my(x, as_prior());
        //new node(x, node::as_next());
        //::std::cout << i << ::std::endl;
    }

    while(!x.empty())
    {
        my &r = x.get_next();
        r.disconnect();
        delete &r;
    }
}
