#include <citylizard/meta/const.hpp>
#include <citylizard/meta/bool.hpp>
#include <boost/mpl/identity.hpp>

namespace M = ::citylizard::meta;
namespace BM = ::boost::mpl;

#define CITYLIZARD_META_ASSERT(X) \
    static_assert(::boost::mpl::identity<decltype(X)>::type::value, #X)

// 0 == 0
// CITYLIZARD_META_ASSERT((M::const_<int, 0>() == M::const_<int, 0>()));
static_assert(M::const_<int, 0>::equal<M::const_<int, 0> >::value, "0 == 0");
// CITYLIZARD_META_ASSERT((M::const_<int, 0>() == ::std::integral_constant<int, 0>()));
static_assert(M::const_<int, 0>::equal< ::std::integral_constant<int, 0> >::value, "0 == 0");
// !(0 == 1)
// CITYLIZARD_META_ASSERT(!(M::const_<int, 0>() == M::const_<int, 1>()));
static_assert(M::const_<int, 0>::equal<M::const_<int, 1> >::not_::value, "! (0 == 1)");
// !(0 != 0)
// CITYLIZARD_META_ASSERT(!(M::const_<int, 0>() != M::const_<int, 0>()));
static_assert(M::const_<int, 0>::not_equal<M::const_<int, 0> >::not_::value, "! (0 != 0)");
// 0 != 1
// CITYLIZARD_META_ASSERT((M::const_<int, 0>() != M::const_<int, 1>()));
static_assert(M::const_<int, 0>::not_equal<M::const_<int, 1> >::value, "0 != 1");

// 0 == (int)short(0)
// CITYLIZARD_META_ASSERT((M::const_<int, 0>() == M::short_<0>().cast_<int>()));
static_assert(M::const_<int, 0>::equal<M::short_<0>::cast<int> >::value, "0 == (int)short(0)");

// 1 + 2 == 3
// CITYLIZARD_META_ASSERT((M::int_<1>() + M::int_<2>() == M::int_<3>()));
static_assert(M::int_<1>::add_c<2>::equal_c<3>::value, "1 + 2 == 3");
// (0x10 >> 4) & 1
// CITYLIZARD_META_ASSERT(M::int_<0x10>()[M::int_<4>()]);
static_assert(M::int_<0x10>::at_c<4>::value, "(0x10 >> 4) & 1");
// CITYLIZARD_META_ASSERT(!M::int_<0x10>()[M::int_<2>()]);
static_assert(M::int_<0x10>::at_c<2>::not_v, "!((0x10 >> 2) & 1)");
// CITYLIZARD_META_ASSERT(M::int_<0x10>() > M::int_<3>());
static_assert(M::int_<0x10>::greater_c<3>::value, "0x10 > 3");