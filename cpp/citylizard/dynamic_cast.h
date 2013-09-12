#pragma once

namespace citylizard
{
    template<class Source>
    class dynamic_cast_t
    {
    public:

        explicit dynamic_cast_t(Source *source): _source(source)
        {
        }

        template<class Dest>
        operator Dest *() const
        {
            return dynamic_cast<Dest *>(_source);
        }

    private:

        Source * const _source;

        dynamic_cast_t &operator=(dynamic_cast_t const &);

    };

    template<class Source>
    dynamic_cast_t<Source> dynamic_cast_(Source *source)
    {
        return dynamic_cast_t<Source>(source);
    }

}
