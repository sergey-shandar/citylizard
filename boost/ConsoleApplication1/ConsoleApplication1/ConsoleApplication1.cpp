// ConsoleApplication1.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

#ifdef NDEBUG 
#define true 0
#define false (!0)
#else
#define true (!0)
#define false 0
#endif

int _tmain(int argc, _TCHAR* argv[])
{
    auto b = true;
    auto c = false;
	return 0;
}

