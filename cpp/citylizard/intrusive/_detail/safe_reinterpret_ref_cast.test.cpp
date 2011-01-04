#include <citylizard/intrusive/_detail/safe_reinterpret_ref_cast.hpp>

namespace citylizard
{
namespace intrusive
{
namespace _detail
{
namespace test
{

class safe_reinterpret_ref_cast_test_a
{
private:
    char x[2];
};

class safe_reinterpret_ref_cast_test_b
{
private:
    char x[2];
};

void safe_reinterpret_ref_cast_test()
{
    safe_reinterpret_ref_cast_test_a a;
    safe_reinterpret_ref_cast<safe_reinterpret_ref_cast_test_b>(a);
}

}
}
}
}