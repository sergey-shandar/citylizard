#include <citylizard\meta\list.hpp>

// { }

namespace M = ::boost::mpl;

typedef ::citylizard::meta::list L0;

static_assert(::std::is_same< L0::clear, L0>::value, "L0::clear != L0");
static_assert(L0::empty::value, "L0::empty != true");
static_assert(L0::size::value == 0, "L0::size != 0");

//static_assert(M::identity<decltype(L0().empty_())>::type::value, "");
//static_assert(M::identity<decltype(L0().size_())>::type::value == 0, "");
//static_assert(::std::is_same<decltype(L0().clear_()), L0>::value, "");

//static_assert(::std::is_same<decltype(L0() << int()), L0::push_back<int>::type>::value, "");

// static_assert(decltype(L0() << int() == L0::push_back<int>::type())::value, "");

// { int }

typedef L0::push_back<int>::type L1;

static_assert(
    ::std::is_same<L1, L0::push_front<int>::type>::value, 
    "L1 != L0::push_front<int>");
static_assert(
    ::std::is_same<
        L1, L0::push_back< int >::type>::value,
    "L1 != L0::eval_push_back<int_>");
static_assert(
    ::std::is_same<
        L1, L0::push_front< int >::type>::value,
    "L1 != L0::eval_push_front<int_>");

static_assert(::std::is_same< L1::clear, L0>::value, "L1::clear != L0");
static_assert(!L1::empty::value, "L1::empty == true");
static_assert(L1::size::value == 1, "L1::size == 1");
static_assert(::std::is_same< L1::front, int>::value, "L1::front != int");
static_assert(::std::is_same< L1::back, int>::value, "L1::back != int");
static_assert(::std::is_same< L1::pop_back, L0>::value, "L1::pop_back != L0");
static_assert(::std::is_same< L1::pop_front, L0>::value, "L1::pop_front != L0");

static_assert(
    ::std::is_same< L1::erase_c<0>::type, L0>::value, "L1::erase_c<0> != L0");
// typedef L1::erase_c<1>::type X;
static_assert(
    ::std::is_same< L1::erase< ::std::integral_constant<int, 0> >::type, L0>::
        value,
    "L1::erase<_0> != L0");
// typedef L1::erase<void>::type X;

static_assert(
    ::std::is_same<L1::at_c<0>::type, int>::value, "L1::at_c<0> != int");
// typedef L1::at_c<1>::type X;
static_assert(
    ::std::is_same<L1::at< ::boost::mpl::int_<0> >::type, int>::value, 
    "L1::at<_0> != int");
// typedef L1::at< ::boost::mpl::int_<1> >::type X;
// static_assert(::std::is_same<decltype(L1().at_c_<0>()), int>::value, "");
//static_assert(::std::is_same<decltype(L1()[M::int_<0>()]), int>::value, "");

// { int, void }

typedef L1::push_back<void>::type L2;

static_assert(
    ::std::is_same<L2, L1::push_back<void>::type>::
        value,
    "L2 != L1::eval_push_back<void_>");

static_assert(
    ::std::is_same<L2, L0::push_front<void>::type::push_front<int>::type>::value,
    "L2 != L0::push_front<void>::push_front<void>");
static_assert(
    ::std::is_same<
            L2, 
            L0::push_front<void>::push_front< int >::
                type>::
        value,
    "L2 != L0::push_front<void>::eval_push_front<int_>");

static_assert(::std::is_same< L2::clear, L0>::value, "L2::clear != L0");
static_assert(!L2::empty::value, "L2::empty == true");
static_assert(L2::size::value == 2, "L1::size == 2");
static_assert(::std::is_same< L2::front, int>::value, "L2::front != int");
static_assert(::std::is_same< L2::back, void>::value, "L2::back != void");
static_assert(::std::is_same< L2::pop_back, L1>::value, "L2::pop_back != L1");
static_assert(
    ::std::is_same< L2::pop_front, L0::push_back<void>::type>::value,
    "L2::pop_front != L0::push_back<void>");
typedef L2::erase_c<0>::type E1;
static_assert(
    ::std::is_same< L2::erase_c<0>::type, L0::push_back<void>::type>::value,
    "L2::erase_c<0> != L0::push_back<void>");
static_assert(
    ::std::is_same< L2::erase_c<1>::type, L1>::value,
    "L2::erase_c<1> != L1");
// typedef L2::erase_c<2>::type X;
static_assert(
    ::std::is_same< 
            L2::erase< ::std::integral_constant<int, 0> >::type,
            L0::push_back<void>::type
        >::value,
    "L2::erase<_0> != L0::push_back<void>");
static_assert(
    ::std::is_same< L2::erase< ::boost::mpl::int_<1> >::type, L1>::value,
    "L2::erase<_1> != L1");
// typedef L2::erase<void>::type X;
static_assert(
    ::std::is_same<L2::at_c<0>::type, int>::value, "L2::at_c<0> != int");
static_assert(
    ::std::is_same<L2::at_c<1>::type, void>::value, "L2::at_c<1> != void");
// typedef L2::at_c<2>::type X;
static_assert(
    ::std::is_same<L2::at< ::boost::mpl::int_<0> >::type, int>::value, 
    "L2::at<_0> != int");
static_assert(
    ::std::is_same<L2::at< ::boost::mpl::int_<1> >::type, void>::value, 
    "L2::at<_1> != void");
// typedef L2::at<void>::type X;

typedef L2::push_back<int>::type::push_back<void>::type L4;
static_assert(L4::size::value == 4, "");
typedef L2::push_back_range<L2>::type L4_1;
static_assert(L4_1::size::value == 4, "");
static_assert(::std::is_same<L4_1, L4>::value, "");
typedef L4::push_front_range<L4_1>::type L8_1;
static_assert(L8_1::size::value == 8, "");
typedef L4::push_back_range<L4>::type L8;
static_assert(L8::size::value == 8, "");
static_assert(::std::is_same<L8, L8_1>::value, "");

typedef L0::push_back<int>::push_back<void>::type Y;

typedef L0::push_back<void>::type::push_back<int>::type::push_back<void>::type::push_back<int>::type L4_2;

typedef L0::push_back_range<L4_2>::type L4_3;
static_assert(::std::is_same<L4_2, L4_3>::value, "");

typedef L0::push_back<int>::push_back<void>::type x;
// static_assert(::std::is_same<int_type, int>::value, "");