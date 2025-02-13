using System.Text;

namespace Kinetic.IO;

public delegate bool LineLimiter(string line);

public class StringIO
{

	public static string GetCompression(ICollection<string> strs, LineLimiter limiter)
	{
		StringBuilder sb = new StringBuilder();

		foreach(string s in strs)
		{
			if(!limiter.Invoke(s))
			{
				continue;
			}

			sb.Append(s);
			sb.AppendLine();
		}

		return sb.ToString();
	}

	public static string[] ReadArray(FileHandle file, LineLimiter limiter = null)
	{
		List<string> list = new List<string>();

		using(StreamReader r = new StreamReader(file.Path))
		{
			string line;

			while((line = r.ReadLine()) != null)
			{
				if(limiter != null && !limiter.Invoke(line)) continue;
				list.Add(line);
			}
		}

		return list.ToArray();
	}

	public static string Read(FileHandle file)
	{
		return File.ReadAllText(file.Path);
	}

	public static void WriteArray(FileHandle file, string[] arr)
	{
		using(StreamWriter r = new StreamWriter(file.Path))
		{
			foreach(string s in arr)
			{
				r.WriteLine(s);
			}
		}
	}

	public static void Write(FileHandle file, string s)
	{
		File.WriteAllText(file.Path, s);
	}

}
