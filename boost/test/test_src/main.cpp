#include <boost/atomic.hpp>
#include <boost/chrono.hpp>
// #include <boost/context/all.hpp>
// #include <boost/coroutine/all.hpp>
#include <boost/date_time.hpp>
#include <boost/exception/all.hpp>
#include <boost/archive/xml_woarchive.hpp>

#if defined(_M_X64)
#define _MPI_LIB_FOLDER "amd64"
#elif defined(_M_IX86)
#define _MPI_LIB_FOLDER "i386"
#endif

#pragma comment(lib, _MPI_LIB_FOLDER "/msmpi.lib")

int main()
{
    return 0;
}