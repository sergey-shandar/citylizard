#include <citylizard/intrusive/_detail/down_cast.hpp>

namespace citylizard_com
{
namespace intrusive
{
namespace _detail
{
namespace test
{

class down_cast_test_a
{
};

class down_cast_test_b: private down_cast_test_a
{
};

template<class T>
class down_cast_test_t
{
public:
    typedef T element_type;
};

void down_cast_test()
{
    down_cast<down_cast_test_t<down_cast_test_a> >(
        down_cast_test_t<down_cast_test_a>());
}

}
}
}
}