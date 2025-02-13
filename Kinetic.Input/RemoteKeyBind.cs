namespace Kinetic.Input;

public class RemoteKeyBind : RemoteKey
{

	RemoteKey observer;

	public RemoteKeyBind(RemoteKeyID code)
	{
		Reset(code);
	}

	public void Consume()
	{
		observer.Consume();
	}

	public int HoldTime()
	{
		return observer.HoldTime();
	}

	public bool Pressed()
	{
		return observer.Pressed();
	}

	public bool Holding()
	{
		return observer.Holding();
	}

	public bool DoublePressed()
	{
		return observer.DoublePressed();
	}

	public void Reset(RemoteKeyID code)
	{
		observer = RemoteKeyboard.Global.Observe(code);
	}

	public void Reset(int code)
	{
		observer =  RemoteKeyboard.Global.Observe(code);
	}

}
