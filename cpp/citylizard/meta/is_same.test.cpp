#include <citylizard/meta/is_same.hpp>

static_assert(citylizard::meta::is_same<int, int>::value, "int == int");