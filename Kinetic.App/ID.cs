using Kinetic.IO;

namespace Kinetic.App;

public delegate FileHandle FileConverter(ID idt);

public readonly struct ID
{

	public static string Cure(string path)
	{
		if(path == null) path = "";

		if(!path.Contains(':'))
		{
			return DefaultNamespace + ":" + path;
		}

		return path;
	}

	public static FileConverter Converter;
	public const int NullId = -1;
	public static string DefaultNamespace = "morinia";

	public readonly string Space;
	public readonly string Key;
	public readonly int Id;

	public ID(string space, string key, int id = NullId)
	{
		Space = space ?? "";
		Key = key ?? "";
		Id = id;
	}

	public ID(string path, int id = NullId)
	{
		if(path == null) path = "";

		if(!path.Contains(':'))
		{
			Space = DefaultNamespace;
			Key = path;
		}
		else
		{
			string[] arr = path.Split(':');
			Space = arr[0];
			Key = arr[1];
		}
		Id = id;
	}

	public ID Relocate(string prefix, string suffix)
	{
		return new ID(Space, prefix + Key + suffix);
	}

	public bool Insideof(string path)
	{
		return Key.StartsWith(path);
	}

	public override bool Equals(object obj)
	{
		if(obj is not ID identity)
		{
			return false;
		}

		return this == identity;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Space, Key);;
	}

	public static bool operator ==(ID i1, ID i2)
	{
		return i1.Key == i2.Key && i1.Space == i2.Space;
	}

	public static bool operator !=(ID i1, ID i2)
	{
		return !(i1 == i2);
	}

	public string Full => ToString();

	public override string ToString()
	{
		return Space + ":" + Key;
	}

	public static implicit operator int(ID idt)
	{
		return idt.Id;
	}

	public static implicit operator string(ID idt)
	{
		return idt.Full;
	}

	public FileHandle File => Converter(this);

}
