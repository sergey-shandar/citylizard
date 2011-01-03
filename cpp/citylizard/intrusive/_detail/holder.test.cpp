#include <citylizard/intrusive/_detail/holder.hpp>

namespace citylizard_com
{
namespace intrusive
{
namespace _detail
{
namespace test
{

class holder_test_type
{
};

}
}

namespace user
{

template<>
class traits<_detail::test::holder_test_type, void>
{
public:
    class bad_ref
    {
    };
};

}

namespace _detail
{
namespace test
{

void holder_test()
{
    holder<holder_test_type> x;
}

}
}
}
}