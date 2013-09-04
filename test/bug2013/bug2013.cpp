// https://connect.microsoft.com/VisualStudio/feedback/details/799463/c-cli-template-specialization
#include <type_traits>

generic<class T> where T: value class
value class C {};

//

template<class T>
void f(T t) {}

template<class T>
void f(T t) {}

//

template<class T>
void x(T t) {}

generic<class T> where T: value class 
void x(C<T>) {}

//

int main()
{
	f(C<int>());
	// x(C<int>());
}
