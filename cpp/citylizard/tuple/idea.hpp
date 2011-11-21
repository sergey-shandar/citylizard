namespace _detail
{
    template<class Config>
    class tuple_traits
    {
    public:

        class struct_t
        {
        public:

            tuple<typename Config::pop_back> pop_back_value;

            back_t back_value;

            front_t const &front() const
            {
                ...
            }

        };

        template<class T>
        static typename push_back_t<T>::type push_back(tuple const &this_, T const &value)
        {
            return tuple<C::push_back<T>::type>(this_, value);
        }

    };

}

template<class Config>
class tuple_optional_t
{
public:

    back_t back() const
    {
        return this->_struct.back_value;
    }

    ...

protected:

    struct_t _struct;

};

template<>
class tuple_optional_t<meta::_detail::null>
{
};

template<class Config>
class tuple_t: public tuple_optional_t<Config>
{
private:

    typedef _detail::tupe_traits<Config> traits_t;

public:

    tuple_t() {}
    
    explicit tuple_t(typename traits_t::one_t one_value): optional_t(one_value) {}
    
    tuple_t(
        typename traits_t::pop_back_t pop_back_value, 
        typename traits_t::back_t back_value)
    :   
        optional_t(pop_back_value, back_value)
    {
    }

    template<class T>
    typename push_back_t<T>::type push_back(T value) const
    {
        return traits_t::push_back(*this, value);
    }

};

typedef tuple_t<meta::_detail::null> tuple;