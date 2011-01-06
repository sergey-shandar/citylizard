#pragma once

#include <citylizard/cast/_detail/base.hpp>

namespace citylizard
{
namespace cast
{

namespace _detail
{

class explicit_
{
public:
	template<class To>
	class traits
	{
	public:
		template<class From>
		static To apply(From const &f)
		{
			return To(f);
		}
	};
}; // class explicit__

} // namespace _detail

typedef _detail::base<_detail::explicit_> explicit_;

} // namespace cast
} // namespace citylizard

