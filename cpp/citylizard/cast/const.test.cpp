#include <citylizard/cast/const.hpp>

#include <boost/test/unit_test.hpp>

BOOST_AUTO_TEST_CASE(cast_const_test)
{
    using namespace citylizard::cast;

    int i1 = 'a';
    int const &i2 = i1;
    int &i3 = const_::ref(i2);
    i3 = 2;
}