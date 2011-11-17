#include <citylizard/binary/b.hpp>

using namespace citylizard::binary;

static_assert(_1111::value == 0xF, "1111");
static_assert(_1111::_1110::value == 0xFE, "1111_1110");
static_assert(_1101::_1111::_1110::value == 0xDFE, "1101_1111_1110");
static_assert(
    _1101::_1111::_1110::_1100::value == 0xDFEC, "1101_1111_1110_1100");
static_assert(
    _1011::_1101::_1111::_1110::_1100::value == 0xBDFEC, 
    "1011_1101_1111_1110_1100");
static_assert(
    _1011::_1101::_1111::_1110::_1100::_1010::value == 0xBDFECA, 
    "1011_1101_1111_1110_1100_1010");
static_assert(
    _1001::_1011::_1101::_1111::_1110::_1100::_1010::value == 0x9BDFECA, 
    "1001_1011_1101_1111_1110_1100_1010");
