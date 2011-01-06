#pragma once

#include <boost/type_traits/remove_const.hpp>
#include <citylizard/meta/const.hpp>
#include <citylizard/cast/detail/base.hpp>

#pragma warning(push)
// 'variable' : pointer truncation from 'type' to 'type'
#pragma warning(disable: 4311)
// 'operation' : conversion from 'type1' to 'type2' of greater size
#pragma warning(disable: 4312)

namespace citylizard
{
namespace cast
{

namespace _detail
{

class safe_reinterpret
{
public:
	template<class To>
	class traits
	{
	public:
		template<class From>
		static To apply(From &f)
		{
			BOOST_STATIC_ASSERT(sizeof(To) == sizeof(From));
			return reinterpret_cast<To>(f);
		}
	};

	template<class To>
	class traits<To *>
	{
	public:
		template<class From>
		static To *apply(From &f)
		{
			BOOST_STATIC_ASSERT(sizeof(To *) == sizeof(From));
			return reinterpret_cast<To *>(f);
		}
		template<class From>
		static To *apply(From *f)
		{
			BOOST_STATIC_ASSERT(sizeof(To) == sizeof(From));
			return reinterpret_cast<To *>(f);
		}
	};
}; // class safe_reinterpret

} // namespace _detail

typedef _detail::base<_detail::safe_reinterpret> safe_reinterpret;

} // namespace cast
} // namespace citylizard

#pragma warning(pop)

