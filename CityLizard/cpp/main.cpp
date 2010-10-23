#include <string>
#include <iostream>

// library

class default_
{
public:
	template<class T>
	operator T() const
	{
		return T();
	}
};

class element
{
public:

	class header
	{
	public:

		bool empty;
		wchar_t const *namespace_;
		wchar_t const *name;

		header(bool Empty, wchar_t const *Namespace, wchar_t const *Name):
			empty(Empty), namespace_(Namespace), name(Name)
		{
		}

	private:
		header &operator=(header const &);
	};

	void write(::std::wostream &o, bool xmlns = true) const
	{
		o << L"<" << this->h.name;
		if(xmlns)
		{
			o << L"xmlns=\"" << this->h.namespace_ << L"\"";
		}
		if(this->h.empty)
		{
			o << L" />";
		}
		else
		{
			o << L">" << L"</" << this->h.name << L">";
		}
	}

protected:

	element(header H):
		h(H)
	{
	}

private:

	header const h;

	element &operator=(element const &);

};

inline ::std::wostream &operator<<(::std::wostream &o, element const &e)
{
	e.write(o);
	return o;
}

// generated code:

namespace xhtml
{
	class T
	{
	public:

		class html;
		class head;
		class body;

		class html: public element
		{
		public:

			html(): element(header(
				false, L"http://www.w3.org/1999/xhtml", L"html"))
			{
			}

			class _0;
			class _1;

			class _0
			{
			public:
				_1 operator[](head head)
				{
					return _1();
				}
			};

			class _1
			{
			public:
				html operator[](body body)
				{
					return html();
				}
			};
		};

		class head: public element
		{
		public:
			head(): element(header(
				false, L"http://www.w3.org/1999/xhtml", L"head"))
			{
			}
		};

		class body: public element
		{
		public:
			body(): element(header(
				false, L"http://www.w3.org/1999/xhtml", L"body"))
			{
			}
		};
	};

	static T::html::_0 html;
	static T::head head;
	static T::body body;
}

// user's code

using namespace xhtml;

class My
{
public:

	static T::html generate()
	{
		return
			html[head][body];
	}
};

int main()
{
	T::html h = My::generate();
	::std::wcout << h;
}