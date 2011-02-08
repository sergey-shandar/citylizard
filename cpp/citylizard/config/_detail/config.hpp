#pragma once

#include <boost/config.hpp>

#ifndef BOOST_MSVC

#define nullptr 0

#define override

template<class T>
class __uuidof_t;

#define __uuidof(x) __uuidof_t<x>::uuid

#define __assume(x) BOOST_ASSERT(x)

#endif
