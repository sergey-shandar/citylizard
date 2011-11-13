#include <citylizard/cast/reinterpret.hpp>

#include <boost/test/unit_test.hpp>

BOOST_AUTO_TEST_CASE(cast_reinterpret_test)
{
    using namespace citylizard::cast;
    //
    ::std::ptrdiff_t i1 = 'a';
    int* p1 = reinterpret::value(i1);
}