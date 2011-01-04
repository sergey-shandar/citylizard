#include <citylizard/intrusive/_detail/assume.hpp>

#include <boost/test/unit_test.hpp>

BOOST_AUTO_TEST_CASE(assume_test)
{
    CITYLIZARD_INTRUSIVE_DETAIL_ASSUME(true);
}
