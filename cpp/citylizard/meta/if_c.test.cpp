#include <citylizard/meta/if_c.hpp>
#include <citylizard/meta/if.hpp>
#include <citylizard/meta/const.hpp>
#include <citylizard/meta/bool.hpp>

#include <boost/type_traits/is_same.hpp>
#include <boost/static_assert.hpp>
#include <boost/mpl/identity.hpp>

namespace C = ::citylizard_com::meta;

static_assert(::std::is_same<C::if_c<true>::type, void>::value, "");
static_assert(!::std::is_same<C::if_c<true>::type, int>::value, "");
static_assert(::std::is_same<C::if_c<true, char>::type, char>::value, "");
static_assert(
	::boost::is_same<C::if_c<false, char>::else_<int>::type, int>::value, "");
static_assert(
	::boost::
		is_same<
			C::if_c<false, char>::else_if_c<true, void>::else_<int>::type, 
			void>::
		value,
	"");
static_assert(
	::boost::
		is_same<
			C::if_c<false, char>::
				else_if_c<false, void>::x::
				else_if_c<true, long>::
				else_<int>::
				type,
			long>::
		value,
	"");
static_assert(
	::boost::
		is_same<
			C::if_c<true, char>::else_if_c<true, void>::else_<int>::type, 
			char>::
		value, 
	"");

#define CITYLIZARD_COM_META_ASSERT(X) \
    static_assert(::boost::mpl::identity<decltype(X)>::type::value, #X)

CITYLIZARD_COM_META_ASSERT(
    C::if__(C::true__, C::true__, C::false__).type_());
CITYLIZARD_COM_META_ASSERT(
    !C::if__(C::false__, C::true__, C::false__).type_());
CITYLIZARD_COM_META_ASSERT(
    C::if__(C::true__, C::true__).else__(C::false__).type_());
CITYLIZARD_COM_META_ASSERT(!
    C::if__(C::false__, C::true__).
    else_if_(C::true__, C::false__).
    else__(C::true__).
    type_());