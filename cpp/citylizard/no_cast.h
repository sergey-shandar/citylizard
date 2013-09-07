#pragma once

namespace citylizard
{
namespace clr
{
    using namespace std;

    template<class From>
    class no_cast_t
    {
    public:

        explicit no_cast_t(From from): _from(from) {}

        template<class To>
        operator To() const
        {
            static_assert(is_same<From, To>::value, "From != To");
            return _from;
        }

    private:

        From const _from;

        no_cast_t &operator=(no_cast_t const &);
    };

    template<class From>
    no_cast_t<From> no_cast(From from)
    {
        return no_cast_t<From>(from);
    }
}
}
