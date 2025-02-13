using Kinetic.App;
using Kinetic.Input;

namespace Kinetic.OpenGL;

public class OGLRemoteKey : RemoteKey
{

	public static long InputCheckTicks;
	byte Press;

	long PressOccur = -1;

	public void Consume()
	{
		Press = 0;
		PressOccur = -1;
	}

	public int HoldTime()
	{
		if(Press == 0)
		{
			return 0;
		}

		return (int) (InputCheckTicks - PressOccur - 1);
	}

	public bool Pressed()
	{
		return Press != 0 && (PressOccur == InputCheckTicks || HoldTime() > Application.MaxTps * 2);
	}

	public bool Holding()
	{
		return Press != 0;
	}

	public void Fire(byte i)
	{
		Press = i;
		PressOccur = InputCheckTicks;
	}

	public bool DoublePressed()
	{
		return Press == 2;
	}

}
