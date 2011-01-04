#pragma once

#include <boost/type_traits/is_reference.hpp>

namespace citylizard
{
namespace cast
{
namespace _detail
{

template<class C>
class base
{
public:

	template<class T>
	class to
	{
	public:
		template<class F>
		static T apply(F const &f)
		{
			return C::template traits<T>::apply(f);
		}
	};

	template<class T>
	class to<T &>
	{
	public:
		template<class F>
		static T &apply(F &f)
		{
			return C::template traits<T &>::apply(f);
		}
	};

	template<class F>
	class value_t
	{
	public:

		explicit value_t(F const &f):
			f(f)
		{
		}

		template<class T>
		operator T() const
		{
			static_assert(!::boost::is_reference<T>::value);
			return C::template traits<T>::apply(this->f);
		}

	private:

		F const f;

		value_t &operator=(value_t const &);

	}; // class value_t

	template<class F>
	static value_t<F> value(F const &f)
	{
		return value_t<F>(f);
	}

	template<class F>
	class ref_t
	{
	public:

		explicit ref_t(F &f):
			f(f)
		{
		}

		template<class T>
		operator T &() const
		//	CBEAR_BERLIOS_DE_CONFIG_DISABLE_SELF_CAST(T, ref_t)
		{
			return C::template traits<T &>::apply(this->f);
		}

	private:

		F &f;

		ref_t &operator=(ref_t const &);

	}; // class ref_t

	template<class F>
	static ref_t<F> ref(F &f)
	{
		return ref_t<F>(f);
	}

}; // class base

} // namespace _detail
} // namespace cast
} // namespace citylizard

