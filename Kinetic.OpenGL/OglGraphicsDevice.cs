using Kinetic.App;
using Kinetic.Math;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Kinetic.OpenGL;

public class OglGraphicsDevice : GraphicsDevice
{

	public unsafe Vector2 Size
	{
		get
		{
			GLFW.GetWindowSize(OGLDevice.Window, out int x, out int y);
			return new Vector2(x, y);
		}
		set => GLFW.SetWindowSize(OGLDevice.Window, (int) value.x, (int) value.y);
	}

	public unsafe Vector2 DeviceSize
	{
		get
		{
			VideoMode* vm = GLFW.GetVideoMode(GLFW.GetPrimaryMonitor());
			return new Vector2(vm->Width, vm->Height);
		}
	}

	public unsafe Vector2 Pos
	{
		get
		{
			GLFW.GetWindowPos(OGLDevice.Window, out int x, out int y);
			return new Vector2(x, y);
		}
		set => GLFW.SetWindowPos(OGLDevice.Window, (int) value.x, (int) value.y);
	}

	public unsafe bool Decorated
	{
		set => GLFW.SetWindowAttrib(OGLDevice.Window, WindowAttribute.Decorated, value);
	}

	public unsafe void Maximize()
	{
		GLFW.MaximizeWindow(OGLDevice.Window);
	}

	public unsafe string Title
	{
		set => GLFW.SetWindowTitle(OGLDevice.Window, value);
	}

	public unsafe void SwapBuffer()
	{
		GLFW.SwapBuffers(OGLDevice.Window);
	}

}
