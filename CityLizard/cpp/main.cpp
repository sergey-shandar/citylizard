#include <string>
#include <iostream>
#include <vector>

#include <boost/shared_ptr.hpp>

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

class node
{
public:

	virtual void write(::std::wostream &O, bool Xmlns = true) const = 0;

};

inline ::std::wostream &operator<<(::std::wostream &o, node const &e)
{
	e.write(o);
	return o;
}

class element: public node
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

	void write(::std::wostream &O, bool Xmlns = true) const override
	{
		O << L"<" << this->h.name;
		if(Xmlns)
		{
			O << L"xmlns=\"" << this->h.namespace_ << L"\"";
		}
		if(this->h.empty)
		{
			O << L" />";
		}
		else
		{
			O << L">";
			if(this->part0)
			{
				O << *this->part0;
			}
			for(
				list_t::const_iterator i = this->list.begin(); 
				i != this->list.end(); 
				++i)
			{
				O << **i;
			}
			O << L"</" << this->h.name << L">";
		}
	}

protected:

	element(header H):
		h(H)
	{
	}

		/*
	element(element const &Part0, node const &N): h(Part0.h), part0(Part0)
	{
		this->list.push_back(N);
	}
	*/

private:

	void write_children(::std::wostream &O)
	{
		if(this->part0)
		{
			this->part0->write_children(O);
		}
		for(
			list_t::const_iterator i = this->list.begin(); 
			i != this->list.end(); 
			++i)
		{
			(*i)->write(O, false);
		}
	}

	header const h;

	element &operator=(element const &);

	::boost::shared_ptr<element> part0;

	typedef ::std::vector< ::boost::shared_ptr<node>> list_t;

	list_t list;

};

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

			class _0: public element
			{
			public:

				_0(): element(header(
					false, L"http://www.w3.org/1999/xhtml", L"html"))
				{
				}

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
