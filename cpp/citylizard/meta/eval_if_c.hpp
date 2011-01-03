#pragma once

#include <boost/utility/enable_if.hpp>
#include <boost/mpl/eval_if.hpp>

#include <citylizard/meta/if_c.hpp>

namespace citylizard_com
{
namespace meta
{

/// \brief Lazy compile-time \c if statement by value.
///
/// \sa eval_if, if_, if_c.
template<bool C, class T, class E = ::boost::mpl::na>
class eval_if_c: public if_c<C, typename T::type, typename E::type>
{
};

    /*
/// \brief Lazy compile-time \c if statement by value.
///
/// \sa eval_if, if_, if_c.
template<bool C, class T, class E = _detail::disable_id>
class eval_if_c: public ::boost::mpl::eval_if_c<C, T, E>
{
};

/// \brief Lazy compile-time \c if statement by value. No else.
template<bool C, class T>
class eval_if_c<C, T, _detail::disable_id>: 
    public ::boost::lazy_enable_if_c<C, T>
{
public:

    /// Itself. Workaround for Visual C++.
    typedef eval_if_c self;

    /// \brief Lazy compile-time \c %else_if statement by value
    ///
    /// \sa eval_else_if, else_if, else_if_c.
    template<bool C1, class T1, class E = _detail::disable_id>
    class eval_else_if_c: 
        public eval_if_c<C || C1, ::boost::mpl::eval_if_c<C, T, T1>, E>
    {
    };

    /// \brief Lazy compile-time \c %else_if statement by type.
    ///
    /// \sa eval_else_if_c, else_if, else_if_c.
    template<class C1, class T1, class E = _detail::disable_id>
    class eval_else_if:
        public eval_else_if_c<C1::value, T1, E>
    {
    };

    /// \brief Compile-time \c %else_if statement by value.
    ///
    /// \sa eval_else_if_c, eval_else_if, else_if.
    template<bool C1, class T1 = void, class E = _detail::disable>
    class else_if_c:
        public eval_else_if_c<C1, ::boost::mpl::identity<T1>, ::boost::mpl::identity<E> >
    {
    };

    /// \brief Compile-time \c %else_if statement by type.
    ///
    /// \sa eval_else_if_c, eval_else_if, else_if_c.
    template<class C1, class T1 = void, class E = detail::disable>
    class else_if: 
        public eval_else_if_c<C1::value, ::boost::mpl::identity<T1>, ::boost::mpl::identity<E> >
    {
    };

    /// \brief Lazy compile-time \c %else statement.
    ///
    /// \sa else_.
    template<class E>
    class eval_else:
        public ::boost::mpl::eval_if_c<C, T, E>
    {
    };

    /// Compile-time \c %else statement.
    ///
    /// \sa eval_else.
    template<class E>
    class else_: 
        public eval_else< ::boost::mpl::identity<E> >
    {
    };

};
*/

}
}
