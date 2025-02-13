namespace Kinetic.IO;

public class BinaryDiCall
{

	public static T Cast<T>(object o)
	{
		switch(o)
		{
			case IBinaryCompound:
			case IBinaryList:
			case string:
			case byte[]:
			case int[]:
			case float[]:
				return (T) o;
			case byte:
			case int:
			case long:
			case float:
			case double:
			case bool:
				return (T) Convert.ChangeType(o, typeof(T));
			default:
				return default;
		}
	}

}
