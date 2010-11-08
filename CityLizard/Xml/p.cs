public abstract class Node
{
	public abstract void ToWriter(XmlWriter writer);
}

public interface IQName
{
	QName QName { get; }
}

public interface ICharactedData
{
	string Value { get; }
}

public class Attribute: Node, IQName, ICharacterData
{
	public void ToWriter(XmlWriter writer);
	public QName QName { get; }
	public string Value { get; }
}

namespace Content
{
	public abstract class ContentNode
	{
	}

	public sealed class Comment: ContentNode, ICharacterData
	{
		public void ToWriter(XmlWriter writer);
		public string Value { get; }
	}

	public sealed class Text: ContentNode, ICharacterData
	{
		public void ToWriter(XmlWriter writer);
		public string Value { get; }
	}

	namespace Element
	{
		public abstract class Element: ContetnNode, IQName
		{
		}

		public class Simple: Element, ICharactedData
		{
			public void ToWriter(XmlWriter writer);
			public QName QName { get; }
			public string Value { get; }
		}

		public abstract class Complex: Element
		{
			public abstract IEnumerable<Attribute> Attributes { get; }			
		}

		public class Empty: Complex
		{
			public void ToWriter(XmlWriter writer);
			public QName QName { get; }
			public IEnumerable<Attribute> Attributes { get; }
		}

		public abstract class NotEmpty: Complex
		{
			public abstract IEnumerable<ContentNode> ContentNodes { get; }
			public abstract void Append(Comment comment);
		}

		public class NotMixed: NotEmpty
		{
			public void ToWriter(XmlWriter writer);
			public QName QName { get; }
			public IEnumerable<Attribute> Attributes { get; }
			public IEnumerable<ContentNode> ContentNodes { get; }
			public void Append(Comment comment);
		}

		public class Mixed: NotEmpty
		{
			public void ToWriter(XmlWriter writer);
			public QName QName { get; }
			public IEnumerable<Attribute> Attributes { get; }
			public IEnumerable<ContentNode> ContentNodes { get; }
			public void Append(Comment comment);
			public void Append(Text text);
		}
	}
}