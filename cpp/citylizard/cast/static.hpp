#pragma once

#include <citylizard/cast/_detail/base.hpp>

namespace citylizard
{
namespace cast
{

namespace _detail
{

class static_
{
public:

	template<class To>
	class traits
	{
	public:
		template<class From>
		static To apply(From &f)
		{
			return static_cast<To>(f);
		}
	};

	// Optimization for Microsoft Visual C++.
	template<class To>
	class traits<To &>
	{
	public:
		template<class From>
		static To &apply(From &f)
		{
			__assume(&f != 0);
			return static_cast<To &>(f);
		}
	};

	/* Because of

	4.7 4 
		If the destination type is bool, see 4.12. If the source type is bool, the 
		value false is converted to zero and the value true is converted to one.

	4.12
		An rvalue of arithmetic, enumeration, pointer, or pointer to member type can 
		be converted to an rvalue of type bool. A zero value, null pointer value, or 
		null member pointer value is converted to false; any other value is converted
		to true.
	*/
	template<>
	class traits<bool>
	{
	public:
		template<class From>
		static bool apply(From &f)
		{
			return f != 0;
		}
	};
}; // class static_

} // namespace _detail

typedef _detail::base<_detail::static_> static_;

} // namespace cast
} // namespace citylizard

