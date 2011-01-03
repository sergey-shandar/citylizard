#include <citylizard/meta/const.hpp>
#include <citylizard/meta/bool.hpp>
#include <boost/mpl/identity.hpp>

namespace M = ::citylizard_com::meta;
namespace BM = ::boost::mpl;

#define CITYLIZARD_COM_META_ASSERT(X) \
    static_assert(::boost::mpl::identity<decltype(X)>::type::value, #X)

// 0 == 0
CITYLIZARD_COM_META_ASSERT((M::const_<int, 0>() == M::const_<int, 0>()));
CITYLIZARD_COM_META_ASSERT((M::const_<int, 0>() == ::std::integral_constant<int, 0>()));
// !(0 == 1)
CITYLIZARD_COM_META_ASSERT(!(M::const_<int, 0>() == M::const_<int, 1>()));
// !(0 != 0)
CITYLIZARD_COM_META_ASSERT(!(M::const_<int, 0>() != M::const_<int, 0>()));
// 0 !- 1
CITYLIZARD_COM_META_ASSERT((M::const_<int, 0>() != M::const_<int, 1>()));

// 0 == (int)short(0)
CITYLIZARD_COM_META_ASSERT((M::const_<int, 0>() == M::short_<0>().cast_<int>()));

// 1 + 2 == 3
CITYLIZARD_COM_META_ASSERT((M::int_<1>() + M::int_<2>() == M::int_<3>()));
// (0x10 >> 4) & 1
CITYLIZARD_COM_META_ASSERT(M::int_<0x10>()[M::int_<4>()]);
CITYLIZARD_COM_META_ASSERT(!M::int_<0x10>()[M::int_<2>()]);
CITYLIZARD_COM_META_ASSERT(M::int_<0x10>() > M::int_<3>());
