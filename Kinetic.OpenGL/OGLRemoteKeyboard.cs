using Kinetic.Input;
using Kinetic.Math;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Kinetic.OpenGL;

public class OGLRemoteKeyboard : RemoteKeyboard
{

	public Dictionary<int, OGLRemoteKey> Observers = new Dictionary<int, OGLRemoteKey>();
	public int PCcountdown;
	public string PileChars = "";
	public float Scroll { get => FloatMath.Abs(ScrollUABS); set => ScrollUABS = value; }
	public float ScrollUABS;
	public VaryingVector2 Cursor { get; } = new VaryingVector2();

	public RemoteKey Observe(RemoteKeyID code)
	{
		return Observe((int) code);
	}

	public RemoteKey Observe(int code)
	{
		if(!Observers.ContainsKey(code))
		{
			Observers[code] = new OGLRemoteKey();
		}

		OGLRemoteKey obs = Observers[code];

		return obs;
	}

	public unsafe string ClippedText
	{
		get => GLFW.GetClipboardString(OGLDevice.Window);
		set => GLFW.SetClipboardString(OGLDevice.Window, value);
	}

	public string Text => PileChars;

	public void ConsumeTextInput()
	{
		PileChars = "";
		PCcountdown = 2;
	}

	public void ConsumeCursorScroll()
	{
		Scroll = 0;
	}

	public ScrollDirection ScrollDirection
	{
		get
		{
			if(ScrollUABS > 0)
			{
				return ScrollDirection.UP;
			}

			if(ScrollUABS < 0)
			{
				return ScrollDirection.DOWN;
			}

			return ScrollDirection.NONE;
		}
	}

	public void StartRoll()
	{
		PCcountdown--;
	}

	public void EndRoll()
	{
		OGLRemoteKey.InputCheckTicks++;
	}

}
