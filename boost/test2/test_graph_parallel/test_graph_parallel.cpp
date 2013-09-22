// test_graph_parallel.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#if defined(_M_X64)
#define _MPI_LIB_FOLDER "amd64"
#elif defined(_M_IX86)
#define _MPI_LIB_FOLDER "i386"
#endif

#pragma comment(lib, _MPI_LIB_FOLDER "/msmpi.lib")

int _tmain(int argc, _TCHAR* argv[])
{
	return 0;
}

