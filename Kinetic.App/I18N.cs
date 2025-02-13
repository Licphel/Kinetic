using Kinetic.IO;

namespace Kinetic.App;

public class I18N
{

	public static Dictionary<string, Dictionary<string, IBinaryCompound>> Langs = new();
	public static string LangKey = "english";

	public static void Load(string namespc, string langkey, IBinaryCompound compound)
	{
		if(!Langs.ContainsKey(langkey))
		{
			Langs[langkey] = new Dictionary<string, IBinaryCompound>();
		}
		var dict = Langs[langkey];
		if(dict.ContainsKey(namespc))
		{
			dict[namespc].Merge(compound);
		}
		else
		{
			dict[namespc] = compound;
		}
	}

	public static string GetText(string key, params string[] repmt)
	{
		return GetText(new ID(key), repmt);
	}

	public static string GetText(ID idt, params string[] repmt)
	{
		if(!Langs.ContainsKey(LangKey))
		{
			return idt.Full;
		}

		IBinaryCompound compound = Langs[LangKey][idt.Space];
		string urep = compound.Search<string>(idt.Key) ?? idt.Full;

		int i = 0;
		foreach(string r in repmt)
		{
			urep = urep.Replace("${" + i + "}", r);
		}

		return urep;
	}

}
