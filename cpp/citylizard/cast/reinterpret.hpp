#pragma once

#include <citylizard/cast/_detail/base.hpp>

namespace citylizard
{
namespace cast
{

namespace _detail
{

class reinterpret
{
public:

	template<class To>
	class traits
	{
	public:

		template<class From>
		static To apply(From &f)
		{
			return reinterpret_cast<To>(f);
		}

	};

}; // class reinterpret

} // namespace _detail

typedef _detail::base<_detail::reinterpret> reinterpret;

} // namespace cast
} // namespace citylizard

