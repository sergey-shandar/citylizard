#pragma once

#include <boost/assert.hpp>

#ifdef _DEBUG
#    define CITYLIZARD_INTRUSIVE_DETAIL_ASSUME(x) BOOST_ASSERT(x)
#else
#    define CITYLIZARD_INTRUSIVE_DETAIL_ASSUME(x) __assume(x)
#endif