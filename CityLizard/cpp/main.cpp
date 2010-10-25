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

// // node

class node
{
public:

	void write(::std::wostream &O, bool Xmlns) const
	{
		this->p->write(O, Xmlns);
	}

	class struct_
	{
	public:		
		virtual ~struct_() {}
		virtual void write(::std::wostream &O, bool Xmlns) const = 0;
	};

protected:

	node(struct_ *P): p(P) {}

	::boost::shared_ptr<struct_> p;

};

inline ::std::wostream &operator<<(::std::wostream &O, node const &E)
{
	E.write(O, true);
	return O;
}

template<class T>
class node_of: public node
{
protected:

	::boost::shared_ptr<T> cast() const
	{
		return ::boost::shared_static_cast<T>(this->p);
	}

	node_of(node_of const &P): node(P) {}

	template<class T0>
	explicit node_of(T0 const &P0): node(new T(P0)) {}

	template<class T0, class T1>
	node_of(T0 const &P0, T1 const &P1): node(new T(P0, P1)) {}

};

// // text

namespace detail
{

class text_struct: public node::struct_
{
public:

	text_struct(::std::wstring const &t): value(t)
	{
	}

	void write(::std::wostream &O, bool Xmlns = true) const override
	{
		for(
			::std::wstring::const_iterator i = this->value.begin(); 
			i != this->value.end(); 
			++i)
		{
			wchar_t const c = *i;
			switch(c)
			{
			case L'<':
				O << L"&lt;";
				break;
			case L'>':
				O << L"&gt;";
				break;
			case L'&':
				O << L"&amp;";
				break;
			case L'"':
				O << L"&quot;";
				break;
			default:
				O << c;
			}
		}
	}

private:

	::std::wstring const value;

};

}

class text: public node_of<detail::text_struct>
{
private:
	typedef node_of<detail::text_struct> base;
public:
	text(::std::wstring const &t): base(t)
	{
	}
};

// // element

class element;

namespace detail
{

class element_header
{
public:

	bool const empty;
	wchar_t const *const namespace_;
	wchar_t const *const name;

	element_header(bool Empty, wchar_t const *Namespace, wchar_t const *Name):
		empty(Empty), namespace_(Namespace), name(Name)
	{
	}

private:

	element_header &operator=(element_header const &);

};

class element_struct: public node::struct_
{
public:

	explicit element_struct(element_header const &H): h(H)
	{
	}

	element_struct(
		::boost::shared_ptr<element_struct> const &Part0, node const &N):
		h(Part0->h),
		part0(Part0)
	{
		this->list.push_back(N);
	}

	void write(::std::wostream &O, bool Xmlns = true) const override
	{
		O << L"<" << this->h.name;
		if(Xmlns)
		{
			O << L" xmlns=\"" << this->h.namespace_ << L"\"";
		}
		if(this->h.empty)
		{
			O << L" />";
		}
		else
		{
			O << L">";
			this->write_content(O);
			O << L"</" << this->h.name << L">";
		}
	}

	void add(text t)
	{
		this->list.push_back(t);
	}

private:

	void write_content(::std::wostream &O) const
	{
		if(this->part0)
		{
			this->part0->write_content(O);
		}
		for(
			list_t::const_iterator i = this->list.begin(); 
			i != this->list.end(); 
			++i)
		{
			i->write(O, false);
		}
	}

	element_header const h;

	::boost::shared_ptr<element_struct> part0;

	typedef ::std::vector<node> list_t;

	list_t list;

};

}

class element: public node_of<detail::element_struct>
{
protected:

	explicit element(detail::element_header const &H): 
		base(H)
	{
	}

	element(element const &Part0, node const &N): 
		base(Part0.cast(), N)
	{
	}

	element(element const &P): base(static_cast<base const &>(P))
	{
	}

	void add(::std::wstring const &t)
	{
		this->cast()->add(text(t));
	}

private:

	typedef node_of<detail::element_struct> base;
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

			html(element const &Part0, node const &N): element(Part0, N)
			{
			}

			class _0;
			class _1;

			class _0: public element
			{
			public:

				_0(): element(detail::element_header(
					false, L"http://www.w3.org/1999/xhtml", L"html"))
				{
				}

				_1 operator[](head head)
				{
					return _1(*this, head);
				}
			};

			class _1: public element
			{
			public:

				_1(element const &Part0, node const &N): element(Part0, N)
				{
				}

				html operator[](body body)
				{
					return html(*this, body);
				}
			};
		};

		class head: public element
		{
		public:
			head(): element(detail::element_header(
				false, L"http://www.w3.org/1999/xhtml", L"head"))
			{
			}
		};

		class body: public element
		{
		public:
			body(): element(detail::element_header(
				false, L"http://www.w3.org/1999/xhtml", L"body"))
			{
			}

			body &operator[](::std::wstring const &t)
			{
				this->add(t);
				return *this;
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
			html[head][body[L"Hello world!<>\"&"]];
	}
};

int main()
{
	T::html h = My::generate();
	::std::wcout << h;
}
