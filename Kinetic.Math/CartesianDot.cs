namespace Kinetic.Math;

public class CartesianDot
{

	public static List<List<T>> GetDot<T>(List<List<T>> values)
	{
		List<List<T>> res = null;

		foreach(List<T> list in values)
		{
			List<List<T>> temp = new List<List<T>>();

			if(res == null)
			{
				foreach(T t in list)
				{
					temp.Add(new List<T> { t });
				}

				res = temp;
				continue;
			}

			foreach(T t in list)
			{
				foreach(List<T> rl in res)
				{
					temp.Add(new List<T>(rl) { t });
				}
			}

			res = temp;
		}

		if(res == null)
		{
			return new List<List<T>>();
		}

		return res;
	}

}
