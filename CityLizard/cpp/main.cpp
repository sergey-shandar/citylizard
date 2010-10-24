#include <string>
#include <iostream>
#include <vector>

#include <boost/intrusive_ptr.hpp>

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

	void write(::std::wostream &O, bool Xmlns) const
	{
		this->p->write(O, Xmlns);
	}

	class struct_
	{
	public:		
		virtual ~struct_() {}
		virtual void write(::std::wostream &O, bool Xmlns) const = 0;
		int i;
	};

protected:

	node() {}

	node(struct_ *P): p(P) {}

	::boost::intrusive_ptr<struct_> p;

};

inline ::std::wostream &operator<<(::std::wostream &O, node const &E)
{
	E.write(O, true);
	return O;
}

class element: public node
{
public:

	class header
	{
	public:

		bool const empty;
		wchar_t const *const namespace_;
		wchar_t const *const name;

		header(bool Empty, wchar_t const *Namespace, wchar_t const *Name):
			empty(Empty), namespace_(Namespace), name(Name)
		{
		}

	private:

		header &operator=(header const &);

	};

protected:

	explicit element(header const &H): node(new struct_(H))
	{
	}

	element(element const &Part0, node const &N): node(new struct_(Part0, N))
	{
	}

private:

	class struct_: public node::struct_
	{
	public:

		explicit struct_(header const &H): h(H)
		{
		}

		struct_(element const &Part0, node const &N): 
			h(Part0.get().h),
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

		header const h;

		::boost::intrusive_ptr<struct_> part0;

		typedef ::std::vector<node> list_t;

		list_t list;

	};

	struct_ const &get() const
	{
		return *static_cast<struct_ *>(this->p.get());
	}

	element() {}

};

inline void intrusive_ptr_add_ref(node::struct_ *p)
{
	++(p->i);
}

inline void intrusive_ptr_release(node::struct_ *p)
{
	if(--(p->i) == 0)
	{
		delete p;
	}
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

			html(element const &Part0, node const &N): element(Part0, N)
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
