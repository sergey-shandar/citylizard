#include <citylizard/cast/implicit.hpp>

#include <boost/test/unit_test.hpp>

BOOST_AUTO_TEST_CASE(cast_implicit_test)
{
    using namespace citylizard::cast;

    int i1 = 'a';
    char i3 = implicit::value(i1);
    i3;
}