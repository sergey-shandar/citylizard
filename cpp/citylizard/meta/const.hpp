#pragma once

#include <citylizard/meta/if.hpp>
#include <citylizard/meta/bool_fwd.hpp>
#include <citylizard/meta/_detail/op.hpp>

#pragma warning(push)
// 'operation' : unsafe use of type 'bool' in operation
#pragma warning(disable: 4804)
// unary minus operator applied to unsigned type, result still unsigned
#pragma warning(disable: 4146)

namespace citylizard
{
namespace meta
{
    /// \brief integral constant.
    template<class T, T v>
    class const_
    {
    public:

        /// Itself.
        typedef const_ type;

        /// Value type.
        typedef T value_type;

        /// Value.
        static T const value = v;

        template<class B, template<T b> class X>
        class check: 
            public if_< 
                    ::std::is_same<typename B::value_type, T>, X<B::value> >::
                type
        {
        };

        // =
#if 1
        /// \note template typedef.
        template<T b>
        class assign_c: public const_<T, b> {};

        /// \note template typedef.
        template<class B>
        class assign: public check<B, assign_c> {};

        /// \brief assignment.
        template<class B>
        typename assign<B>::type operator=(B) const;
#endif

        // ==
#if 1
        /// \note template typedef
        template<T b>
        class equal_c: public bool_<v == b> {};

        /// \note template typedef
        template<class B>
        class equal: public check<B, equal_c> {};

        /// \brief equal to.
        template<class B>
        typename equal<B>::type operator==(B) const;
#endif

        // !=
#if 1
        /// \note template typedef
        template<T b>
        class not_equal_c: public bool_<v != b> {};

        /// \note template typedef
        template<class B>
        class not_equal: public check<B, not_equal_c> {};

        /// \brief not equal to.
        template<class B>
        typename not_equal<B>::type operator!=(B) const;
#endif

        // <
#if 1
        /// \note template typedef
        template<T b>
        class less_c: public bool_<_detail::less<T, v, b>::value> {};

        /// \note template typedef
        template<class B>
        class less: public check<B, less_c> {};

        /// \brief not equal to.
        template<class B>
        typename less<B>::type operator<(B) const;
#endif

        // >
#if 1
        /// \note template typedef
        template<T b>
        class greater_c: public bool_<(v > b)> {};

        /// \note template typedef
        template<class B>
        class greater: public check<B, greater_c> {};

        /// \brief greater than.
        template<class B>
        typename greater<B>::type operator>(B) const;
#endif

        // <=
#if 1
        /// \note template typedef
        template<T b>
        class less_equal_c: public bool_<_detail::less_equal<T, v, b>::value> {};

        /// \note template typedef
        template<class B>
        class less_equal: public check<B, less_equal_c> {};

        /// \brief less or equal than
        template<class B>
        typename less_equal<B>::type operator<=(B) const;
#endif

        // >=
#if 1
        /// \note template typedef
        template<T b>
        class greater_equal_c: public bool_<(v >= b)> {};

        /// \note template typedef
        template<class B>
        class greater_equal: public check<B, greater_equal_c> {};

        /// \brief greater or equal than
        template<class B>
        typename greater_equal<B>::type operator>=(B) const;
#endif

        // !
#if 1
        static T const not_v = !v;

        /// \brief logical not.
        typedef const_<T, not_v> not;

        /// \brief logical not.
        not operator!() const;
#endif

        // +A
#if 1
        static T const plus_v = +v;

        /// \brief unary plus.
        typedef const_<T, plus_v> plus;

        /// \brief unary plus.
        plus operator+() const;
#endif

        // -A
#if 1
        static T const minus_v = static_cast<T>(-v);

        /// \brief unary minus
        typedef const_<T, minus_v> minus;

        /// \brief unary minus
        minus operator-() const;
#endif

        // ~
#if 1
        static T const bit_not_v = static_cast<T>(~v);

        /// \brief bit not.
        typedef const_<T, bit_not_v> bit_not;

        /// \brief bit not.
        bit_not operator~() const;
#endif

        // ++
#if 1
        static T const inc_v = static_cast<T>(v + 1);

        /// \brief increment
        typedef const_<T, inc_v> inc;

        /// \brief incerement
        inc operator++() const;
#endif

        // --
#if 1
        static T const dec_v = static_cast<T>(v - 1);

        /// \brief decrement
        typedef const_<T, dec_v> dec;

        /// \brief decrement
        dec operator--() const;
#endif

        // A+B
#if 1
        /// \note template typedef
        template<T b>
        class add_c: public assign_c<v + b> {};

        /// \note template typedef
        template<class B>
        class add: public check<B, add_c> {};


        template<class B>
        typename add<B>::type operator+(B) const;
#endif

        // A-B
#if 1
        // template typedef
        template<T b>
        class sub_c: public assign_c<v - b> {};

        // template typedef
        template<class B>
        class sub: public check<B, sub_c> {};

        template<class B>
        typename sub<B>::type operator-(B) const;
#endif

        // *
#if 1
        // template typedef
        template<T b>
        class mul_c: public assign_c<v * b> {};

        // template typedef
        template<class B>
        class mul: public check<B, mul_c> {};

        template<class B>
        typename mul<B>::type operator*(B) const;
#endif

        // /
#if 1
        // template typedef
        template<T b>
        class div_c: public assign_c<v / b> {};

        // template typedef
        template<class B>
        class div: public check<B, div_c> {};

        template<class B>
        typename div<B>::type operator/(B) const;
#endif

        // %
#if 1
        // template typedef
        template<T b>
        class rem_c: public assign_c<v % b> {};

        // template typedef
        template<class B>
        class rem: public check<B, rem_c> {};

        template<class B>
        typename rem<B>::type operator%(B) const;
#endif

        // &
#if 1
        // template typedef
        template<T b>
        class bit_and_c: public assign_c<v & b> {};

        // template typedef
        template<class B>
        class bit_and: public check<B, bit_and_c> {};

        template<class B>
        typename bit_and<B>::type operator&(B) const;
#endif

        // |
#if 1
        // tempalte typedef
        template<T b>
        class bit_or_c: public assign_c<v & b> {};

        // template typedef
        template<class B>
        class bit_or: public check<B, bit_or_c> {};

        template<class B>
        typename bit_or<B>::type operator|(B) const;
#endif

        // ^
#if 1
        // template typedef
        template<T b>
        class bit_xor_c: public assign_c<v ^ b> {};

        // template typedef
        template<class B>
        class bit_xor: public check<B, bit_xor_c> {};

        template<class B>
        typename bit_xor<B>::type operator^(B) const;
#endif

        // &&
#if 1
        // template typedef
        template<T b>
        class and_c: public assign_c<v && b> {};

        // template typedef
        template<class B>
        class and: public check<B, and_c> {};

        template<class B>
        typename and<B>::type operator&&(B) const;
#endif

        // ||
#if 1
        // template typedef
        template<T b>
        class or_c: public assign_c<v || b> {};

        // template typedef
        template<class B>
        class or: public check<B, or_c> {};

        template<class B>
        typename or<B>::type operator||(B) const;
#endif

        // <<
#if 1
        // template typedef
        template<int b>
        class left_shift_c: public assign_c<_detail::left_shift<T, v, b>::value> {};

        // template typedef
        template<class B>
        class left_shift: public left_shift_c<B::value> {};

        template<class B>
        typename left_shift<B>::type operator<<(B) const;
#endif

        // >>
#if 1
        // template typedef
        template<int b>
        class right_shift_c: public assign_c<(v >> b)> {};

        // template typedef
        template<class B>
        class right_shift: public right_shift_c<B::value> {};

        template<class B>
        typename right_shift<B>::type operator>>(B) const;
#endif

        // []
#if 1
        // template typedef
        template<int b>
        class at_c: public bool_<(v >> b) & 1> {};

        // template typedef
        template<class B>
        class at: public at_c<B::value> {};

        template<class B>
        typename at<B>::type operator[](B) const;
#endif

        // cast
#if 1
        // template typedef
        template<class T1>
        class cast: public const_<T1, v> {};

        template<class T1>
        typename cast<T1>::type cast_() const;
#endif

    };

    // template typedef
    template<int v>
    class int_: public const_<int, v>
    {
    };

    // template typedef
    template<short v>
    class short_: public const_<short, v>
    {
    };

    // template typedef
    template<char v>
    class char_: public const_<char, v>
    {
    };

}
}

#pragma warning(pop)