namespace Kinetic.App;

//all uploaded resources are stored here. you can get them by identity.
public class Loads
{

	public static readonly Dictionary<string, object> ResDict = new Dictionary<string, object>();

	public static void Load<T>(ID key, T o)
	{
		ResDict[key.Full] = o;
	}

	public static void Load<T>(string key, T o)
	{
		ResDict[ID.Cure(key)] = o;
	}

	public static dynamic Get(string key, object defaultv = null)
	{
		return ResDict.GetValueOrDefault(ID.Cure(key), defaultv);
	}

	public static dynamic Get(ID key, object defaultv = null)
	{
		return ResDict.GetValueOrDefault(key.Full, defaultv);
	}

}
