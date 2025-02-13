using Kinetic.IO;

namespace Kinetic.App;

public class Options
{

	public readonly Dictionary<string, Option> Keys = new Dictionary<string, Option>();
	public IBinaryCompound Values = IBinaryCompound.New();

	public void Join(Option option)
	{
		Keys[option.Key] = option;
		option.Group = this;
	}

	public void WriteToLocal(FileHandle file)
	{
		BinaryIO.Write(Values, file);
	}

	public void ReadFromLocal(FileHandle file)
	{
		if(!file.Exists) return;

		Values = BinaryIO.Read(file);

		foreach(Option o in Keys.Values)
		{
			o.Updater.Invoke(null, o.GetValue<object>());
		}
	}

}

public class Option
{

	public delegate void OptionUpdater(dynamic oldvalue, dynamic newvalue);
	public object Defval;
	public Options Group;

	public string Key;
	public OptionUpdater Updater;

	public Option(string keyingroup, object defval, OptionUpdater upd = null)
	{
		Key = keyingroup;
		Defval = defval;
		Updater = upd;
	}

	public T GetValue<T>()
	{
		if(Group.Values.Try(Key, out T val))
		{
			return val;
		}
		return (T) Defval;
	}

	public void SetValue<T>(T value)
	{
		T old = Group.Values.Get<T>(Key);
		Group.Values.Set(Key, value);
		Updater?.Invoke(old, value);
	}

}
