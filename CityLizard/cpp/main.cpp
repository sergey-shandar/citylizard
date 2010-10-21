#include <string>
#include <iostream>

// library

class element
{
public:
	void write(::std::wostream &o) const
	{
		o << L'<' << this->name << L'>' << L'</' << this->name << L'>';
	}
protected:
	element(wchar_t const *Name):
		name(Name)
	{
	}
private:
	wchar_t const *name;
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

			html(): element(L"html")
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
			head(): element(L"head")
			{
			}
		};

		class body: public element
		{
		public:
			body(): element(L"body")
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