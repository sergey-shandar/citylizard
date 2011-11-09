#pragma once

#include <citylizard/meta/const.hpp>

namespace citylizard
{

/// Binary numeral system.
namespace binary
{

namespace _detail
{

template<unsigned int N>
class b: public meta::const_<unsigned int, N>
{
public:

    template<unsigned int N2>
    class b_: public b<N << 4 | N2> {};

    /// 0000 (0x0,  0, 000)
    typedef b_<0>  _0000;
    /// 0001 (0x1,  1, 001)
    typedef b_<1>  _0001;
    /// 0010 (0x2,  2, 002)
    typedef b_<2>  _0010;
    /// 0011 (0x3,  3, 003)
    typedef b_<3>  _0011;
    /// 0100 (0x4,  4, 004)
    typedef b_<4>  _0100;
    /// 0101 (0x5,  5, 005)
    typedef b_<5>  _0101;
    /// 0110 (0x6,  6, 006)
    typedef b_<6>  _0110;
    /// 0111 (0x7,  7, 007)
    typedef b_<7>  _0111;
    /// 1000 (0x8,  8, 010)
    typedef b_<8>  _1000;
    /// 1001 (0x9,  9, 011)
    typedef b_<9>  _1001;
    /// 1010 (0xA, 10, 012)
    typedef b_<10> _1010;
    /// 1011 (0xB, 11, 013)
    typedef b_<11> _1011;
    /// 1100 (0xC, 12, 014)
    typedef b_<12> _1100;
    /// 1101 (0xD, 13, 015)
    typedef b_<13> _1101;
    /// 1110 (0xE, 14, 016)
    typedef b_<14> _1110;
    /// 1111 (0xF, 15, 017)
    typedef b_<15> _1111;
};

}

/// 0000 (0x0,  0, 000)
typedef _detail::b<0>  _0000;
/// 0001 (0x1,  1, 001)
typedef _detail::b<1>  _0001;
/// 0010 (0x2,  2, 002)
typedef _detail::b<2>  _0010;
/// 0011 (0x3,  3, 003)
typedef _detail::b<3>  _0011;
/// 0100 (0x4,  4, 004)
typedef _detail::b<4>  _0100;
/// 0101 (0x5,  5, 005)
typedef _detail::b<5>  _0101;
/// 0110 (0x6,  6, 006)
typedef _detail::b<6>  _0110;
/// 0111 (0x7,  7, 007)
typedef _detail::b<7>  _0111;
/// 1000 (0x8,  8, 010)
typedef _detail::b<8>  _1000;
/// 1001 (0x9,  9, 011)
typedef _detail::b<9>  _1001;
/// 1010 (0xA, 10, 012)
typedef _detail::b<10> _1010;
/// 1011 (0xB, 11, 013)
typedef _detail::b<11> _1011;
/// 1100 (0xC, 12, 014)
typedef _detail::b<12> _1100;
/// 1101 (0xD, 13, 015)
typedef _detail::b<13> _1101;
/// 1110 (0xE, 14, 016)
typedef _detail::b<0xE> _1110;
/// 1111 (0xF, 15, 017)
typedef _detail::b<0xF> _1111;

} // namespace binary
} // namespace citylizard
