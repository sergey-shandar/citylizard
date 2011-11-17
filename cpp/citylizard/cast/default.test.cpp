#include <citylizard/cast/default.hpp>

#include <boost/test/unit_test.hpp>

class MyClass 
{
public:
    MyClass() {}
    explicit MyClass(char) {}
};

BOOST_AUTO_TEST_CASE(cast_default_test)
{
    using namespace citylizard::cast;

    MyClass my3 = default_::value();
}