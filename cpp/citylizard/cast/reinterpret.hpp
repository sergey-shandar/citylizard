#pragma once

#include <citylizard/cast/_detail/base.hpp>

namespace citylizard
{
namespace cast
{

namespace detail
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

}

typedef _detail::base<_detail::reinterpret> reinterpret;

} // namespace cast
} // namespace citylizard

