#include <citylizard/cast/explicit.hpp>

#include <boost/test/unit_test.hpp>

class MyClass 
{
public:
    MyClass() {}
    explicit MyClass(char) {}
};

BOOST_AUTO_TEST_CASE(cast_explicit_test)
{
    using namespace citylizard::cast;
    //
    // MyClass my1 = '5'; // error
    MyClass my = explicit_::value('5');
}