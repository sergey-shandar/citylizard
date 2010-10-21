#include <string>

// generated code:

class X
{
public:

	class T
	{
	public:

		class html;
		class head;
		class body;

		class html
		{
		public:

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

		class head
		{
		};

		class body
		{
		};
	};

	static T::html::_0 html;
	static T::head head;
	static T::body body;
};

// user's code

class My: X
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
}