#include <string>
#include <iostream>
#include <vector>

#include <boost/utility/enable_if.hpp>
#include <boost/shared_ptr.hpp>
#include <boost/make_shared.hpp>

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
		this->_p->write(O, Xmlns);
	}

	class struct_
	{
	public:		
		virtual ~struct_() {}
		virtual void write(::std::wostream &O, bool Xmlns) const = 0;
	};

protected:

	node(::boost::shared_ptr<struct_> const &P): _p(P) {}

	::boost::shared_ptr<struct_> _p;

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
		return ::boost::shared_static_cast<T>(this->_p);
	}

	node_of(node_of const &P): node(P) {}

	template<class T0>
	explicit node_of(T0 const &P0): node(::boost::make_shared<T>(P0)) {}

	template<class T0, class T1>
	node_of(T0 const &P0, T1 const &P1): node(::boost::make_shared<T>(P0, P1))
	{
	}

	template<class T0, class T1, class T2>
	node_of(T0 const &P0, T1 const &P1, T2 const &P2): 
		node(::boost::make_shared<T>(P0, P1, P2))
	{
	}

};

// // text

namespace detail
{

inline void write_text(::std::wostream &O, ::std::wstring const &text)
{
	for(::std::wstring::const_iterator i = text.begin(); i != text.end(); ++i)
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

class text_struct: public node::struct_
{
public:

	text_struct(::std::wstring const &t): value(t)
	{
	}

	void write(::std::wostream &O, bool Xmlns = true) const override
	{
		write_text(O, this->value);
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

class attribute_struct
{
public:

	wchar_t const *const name;

	::std::wstring const value;

	attribute_struct(wchar_t const *name_, ::std::wstring const &value_):
		name(name_), value(value_)
	{
	}

	void write(::std::wostream &O) const
	{
		O << L' ' << this->name << L"=\"";
		write_text(O, this->value);
		O << L'"';
	}
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

	element_struct(
		::boost::shared_ptr<element_struct> const &Part0, 
		wchar_t const *name, 
		::std::wstring const &value): 
		h(Part0->h),
		part0(Part0),
		attribute(::boost::make_shared<attribute_struct>(name, value))
	{
	}

	void write(::std::wostream &O, bool Xmlns = true) const override
	{
		O << L"<" << this->h.name;
		if(Xmlns)
		{
			O << L" xmlns=\"" << this->h.namespace_ << L"\"";
		}
		this->write_attributes(O);
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

	void add(node t)
	{
		this->list.push_back(t);
	}

private:

	void write_attributes(::std::wostream &O) const
	{
		if(this->part0)
		{
			this->part0->write_attributes(O);
		}
		if(this->attribute)
		{
			this->attribute->write(O);
		}
	}

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

	::boost::shared_ptr<attribute_struct> attribute;

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

	element(
		element const &Part0, wchar_t const *Name, ::std::wstring const &Value):
		base(Part0.cast(), Name, Value)
	{
	}

	element(element const &P): base(static_cast<base const &>(P))
	{
	}

	void add(::std::wstring const &t)
	{
		this->cast()->add(text(t));
	}

	void add(element const &e)
	{
		this->cast()->add(e);
	}

private:

	typedef node_of<detail::element_struct> base;
};

template<class R, class T, class A>
R add_attribute(
	A const *a, wchar_t const *name, ::std::wstring const &value)
{
	return R(
		static_cast<element const &>(*static_cast<T const *>(a)), name, value);
}

// generated code:

namespace xhtml
{
	class A
	{
	public:

		template<class T, class R, bool on> 
		class version_t
		{
		};

		template<class T, class R>
		class version_t<T, R, true>
		{
		public:
			R version(::std::wstring const &value)
			{
				return add_attribute<R, T>(this, L"version", value);
			}
		};

		template<class T, class R, bool on> 
		class lang_t
		{
		};

		template<class T, class R>
		class lang_t<T, R, true>
		{
		public:
			R lang(::std::wstring const &value)
			{
				return add_attribute<R, T>(this, L"lang", value);
			}
		};
	};

	class T
	{
	public:

		class html;
		class head;
		class body;
		class div_;
		class p;
		class img;

		class html: public element
		{
		public:

			html(element const &Part0, node const &N): element(Part0, N)
			{
			}

			template<
				bool version_on = true, 
				bool lang_on = true, 
				bool dir_on = true, 
				bool id_on = true, 
				bool space_on = true>
			class _0;
			class _1;

			template<
				bool version_on, 
				bool lang_on, 
				bool dir_on, 
				bool id_on, 
				bool space_on>
			class _0: 
				public element, 
				public A::version_t<
					_0<true, lang_on, dir_on, id_on, space_on>,
					_0<false, lang_on, dir_on, id_on, space_on>, 
					version_on>,
				public A::lang_t<
					_0<version_on, true, dir_on, id_on, space_on>,
					_0<version_on, false, dir_on, id_on, space_on>, 
					lang_on>
			{
			public:

				_0(): element(detail::element_header(
					false, L"http://www.w3.org/1999/xhtml", L"html"))
				{
				}

				_0(
					element const &Part0, 
					wchar_t const *Name, 
					::std::wstring const &Value):
					element(Part0, Name, Value)
				{
				}

				_1 operator()(head head)
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

				html operator()(body body)
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

			body &operator()(::std::wstring const &t)
			{
				this->add(t);
				return *this;
			}

			body &operator()(div_ div_)
			{
				this->add(div_);
				return *this;
			}

			body &operator()(p p)
			{
				this->add(p);
				return *this;
			}

			body &operator()(img img)
			{
				this->add(img);
				return *this;
			}
		};

		class div_: public element
		{
		public:
			div_(): element(detail::element_header(
				false, L"http://www.w3.org/1999/xhtml", L"div"))
			{
			}
		};

		class p: public element
		{
		public:
			p(): element(detail::element_header(
				false, L"http://www.w3.org/1999/xhtml", L"p"))
			{
			}
		};

		class img: public element
		{
		public:
			img(): element(detail::element_header(
				true, L"http://www.w3.org/1999/xhtml", L"img"))
			{
			}
		};
	};

	static T::html::_0<> html;
	static T::head head;
	static T::body body;
	static T::div_ div_;
	static T::p p;
	static T::img img;
}

// user's code

using namespace xhtml;

class My
{
public:

	static T::html generate()
	{
		return
			html.version(L"1.1").lang(L"en")
				(head)
				(body(L"Hello world!<>\"&")(p)(div_)(img));
	}
};

int main()
{
	T::html h = My::generate();
	::std::wcout << h;
}
