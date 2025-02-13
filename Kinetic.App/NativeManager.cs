namespace Kinetic.App;

public class NativeManager
{

	public static NativeManager I0 = new NativeManager();

	readonly List<Response> ToRelease = new List<Response>();

	public void Remind(Response release)
	{
		ToRelease.Add(release);
	}

	public void Free()
	{
		ToRelease.ForEach(r =>
		{
			r.Invoke();
		});
		ToRelease.Clear();
	}

}
