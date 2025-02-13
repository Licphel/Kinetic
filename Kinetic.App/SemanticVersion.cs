namespace Kinetic.App;

public class SemanticVersion : IComparable<SemanticVersion>
{

	public string FullName;
	public long Iteration;
	public string Prefix;
	public bool Snapshot;
	public bool Stable;

	//code is like stable-1.0.0
	public SemanticVersion(string code)
	{
		FullName = code;
		Prefix = code[code.LastIndexOf('-')..];
		Snapshot = Prefix.Contains("Snapshot", StringComparison.OrdinalIgnoreCase);
		Stable = !Snapshot;
		string vcode = code.Substring(code.LastIndexOf('-'));
		string[] vers = vcode.Split('.');
		long sta = 1;
		foreach(string s in vers.Reverse())
		{
			int v = int.Parse(s);
			Iteration += v * sta;
			sta *= 1000;
		}
	}

	public int CompareTo(SemanticVersion other)
	{
		if(other.Iteration < Iteration) return 1;
		if(other.Iteration == Iteration) return 0;
		return -1;
	}

}
