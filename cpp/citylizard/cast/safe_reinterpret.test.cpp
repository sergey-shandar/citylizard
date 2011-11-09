#include <citylizard/cast/safe_reinterpret.hpp>

#include <boost/test/unit_test.hpp>

BOOST_AUTO_TEST_CASE(cast_safe_reinterpret_test)
{
    using namespace citylizard::cast;
    //
    ::ptrdiff_t pd = 3;
    int* p = safe_reinterpret::value(pd);
}