#pragma once

#include <citylizard/meta/_detail/if_c.hpp>
#include <boost/mpl/aux_/na.hpp>

namespace citylizard_com
{
/// Meta-programming.
namespace meta
{

/// \brief Lazy compile-time \c if statement by value.
///
/// \sa eval_if, if_, if_c.
template<bool C, class T = void, class E = ::boost::mpl::na>
class if_c: public _detail::if_c<C, T, E>
{
};

/// \brief Compile-time \c if statement by value, no else.
///
/// \sa eval_if, eval_if_c, if_.
template<bool C, class T>
class if_c<C, T, ::boost::mpl::na>: 
    public _detail::enable_if_c<C, T>
{
public:

    /// Itself. Workaround for Visual C++ and template typedefs.
    typedef if_c x;

    /// \brief Compile-time \c %else_if statement by value.
    ///
    /// \sa eval_else_if_c, eval_else_if, else_if.
    template<bool C1, class T1 = void, class E = ::boost::mpl::na>
    class else_if_c:
        public if_c<C || C1, typename _detail::if_c<C, T, T1>::type, E>
    {
    };

    /// \brief Lazy compile-time \c %else_if statement by value
    ///
    /// \sa eval_else_if, else_if, else_if_c.
    template<bool C1, class T1, class E = ::boost::mpl::na>
    class eval_else_if_c: 
        public else_if_c<C1, typename T1::type, typename E::type>
    {
    };

    /// \brief Lazy compile-time \c %else_if statement by type.
    ///
    /// \sa eval_else_if_c, else_if, else_if_c.
    template<class C1, class T1, class E = ::boost::mpl::na>
    class eval_else_if:
        public else_if_c<C1::value, typename T1::type, typename E::type>
    {
    };

    /// \brief Compile-time \c %else_if statement by type.
    ///
    /// \sa eval_else_if_c, eval_else_if, else_if_c.
    template<class C1, class T1 = void, class E = ::boost::mpl::na>
    class else_if:
        public else_if_c<C1::value, T1, E>
    {
    };

    template<class C1, class T1, class E>
    else_if<C1, T1, E> else_if_(C1, T1, E) const;

    template<class C1, class T1>
    else_if<C1, T1> else_if_(C1, T1) const;

    template<class C1>
    else_if<C1> else_if_(C1) const;

    /// Compile-time \c %else statement.
    ///
    /// \sa eval_else.
    template<class E>
    class else_: 
        public _detail::if_c<C, T, E>
    {
    };

    /// \brief Lazy compile-time \c %else statement.
    ///
    /// \sa else_.
    template<class E>
    class eval_else:
        public else_<typename E::type>
    {
    };

    template<class E>
    else_<E> else__(E);
};

}
}
