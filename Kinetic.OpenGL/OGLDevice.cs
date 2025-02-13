using Kinetic.App;
using Kinetic.Input;
using Kinetic.IO;
using Kinetic.Math;
using Kinetic.Visual;
using OpenTK.Windowing.GraphicsLibraryFramework;
using StbImageSharp;
using GL = OpenTK.Graphics.OpenGL.GL;

namespace Kinetic.OpenGL;

public class OGLDevice
{

	public static unsafe Window* Window;

	public static OGLDeviceSettings Settings;
	public static int Pw, Ph;

	public static void LoadSettings(OGLDeviceSettings s)
	{
		Settings = s;
		Pw = (int) s.Size.x;
		Ph = (int) s.Size.y;
		Application.ScaledSize = new VaryingVector2(Pw, Ph);
	}

	public static unsafe void OpenWindow()
	{
		GLFW.Init();
		GLFW.SwapInterval(1);
		GLFW.DefaultWindowHints();
		GLFW.WindowHint(WindowHintBool.Decorated, Settings.Decorated);
		GLFW.WindowHint(WindowHintBool.Floating, Settings.Floating);
		GLFW.WindowHint(WindowHintBool.Resizable, Settings.Resizable);
		GLFW.WindowHint(WindowHintBool.Maximized, Settings.Maximized);
		GLFW.WindowHint(WindowHintBool.AutoIconify, Settings.AutoIconify);
		GLFW.WindowHint(WindowHintBool.FocusOnShow, true);
		GLFW.WindowHint(WindowHintBool.Visible, false);

		Window = GLFW.CreateWindow(Pw, Ph, Settings.Title, null, null);

		VideoMode* vm = GLFW.GetVideoMode(GLFW.GetPrimaryMonitor());
		float x = (vm->Width - Settings.Size.x) / 2;
		float y = (vm->Height - Settings.Size.y) / 2;
		GLFW.SetWindowPos(Window, (int) x, (int) y);

		if(Settings.Cursor != null)
		{
			ImageResult result = ImageResult.FromMemory(File.ReadAllBytes(Settings.Cursor.Path), ColorComponents.RedGreenBlueAlpha);

			fixed(byte* ptr = result.Data)
			{
				Image cimg = new Image(result.Width, result.Height, ptr);
				Cursor* cptr = GLFW.CreateCursor(cimg, Settings.Hotspot.xi, Settings.Hotspot.yi);
				GLFW.SetCursor(Window, cptr);
			}
		}

		if(Settings.Icons != null)
		{
			List<Image> img = new List<Image>();

			foreach(FileHandle icon in Settings.Icons)
			{
				ImageResult result = ImageResult.FromMemory(File.ReadAllBytes(icon.Path), ColorComponents.RedGreenBlueAlpha);

				fixed(byte* ptr = result.Data)
				{
					Image cimg = new Image(result.Width, result.Height, ptr);
					img.Add(cimg);
				}
			}

			GLFW.SetWindowIcon(Window, new ReadOnlySpan<Image>(img.ToArray()));
		}

		if(Settings.Maximized) GLFW.MaximizeWindow(Window);

		GLFW.MakeContextCurrent(Window);
		GLFW.ShowWindow(Window);
		GL.LoadBindings(new GLFWBindingsContext());

		OnLoad();
	}

	public static unsafe void OnLoad()
	{
		Time.SysNanotimer = () => GLFW.GetTime() * 1000_000_000;
		GraphicsDevice.Global = new OglGraphicsDevice();
		RemoteKeyboard.Global = new OGLRemoteKeyboard();
		SpriteBatch.Global = new OGLSpriteBatch(SpriteBatch.DefaultSize);

		Application.Update += GLFW.PollEvents;

		GLFW.SetWindowCloseCallback(Window, _ =>
		{
			Application.Stop();
		});
		GLFW.SetCharCallback(Window, (_, codepoint) =>
		{
			OGLRemoteKeyboard state = (OGLRemoteKeyboard) RemoteKeyboard.Global;
			if(state.PCcountdown <= 0)
				state.PileChars += (char) codepoint;
		});
		GLFW.SetKeyCallback(Window, (_, key, code, action, mods) =>
		{
			OGLRemoteKeyboard state = (OGLRemoteKeyboard) RemoteKeyboard.Global;
			OGLRemoteKey observer = (OGLRemoteKey) state.Observe((int) key);
			OGLRemoteKey observer2 = (OGLRemoteKey) state.Observe((int) RemoteKeyID.ANY);
			switch(action)
			{
				case InputAction.Press:
					observer.Fire(1);
					observer2.Fire(1);
					break;
				case InputAction.Repeat:
					observer.Fire(2);
					observer2.Fire(2);
					break;
				case InputAction.Release:
					observer.Consume();
					observer2.Consume();
					break;
			}
		});
		GLFW.SetMouseButtonCallback(Window, (_, key, action, mods) =>
		{
			OGLRemoteKeyboard state = (OGLRemoteKeyboard) RemoteKeyboard.Global;
			OGLRemoteKey observer = (OGLRemoteKey) state.Observe((int) key);
			OGLRemoteKey observer2 = (OGLRemoteKey) state.Observe((int) RemoteKeyID.ANY);
			switch(action)
			{
				case InputAction.Press:
					observer.Fire(1);
					observer2.Fire(1);
					break;
				case InputAction.Repeat:
					observer.Fire(2);
					observer2.Fire(2);
					break;
				case InputAction.Release:
					observer.Consume();
					observer2.Consume();
					break;
			}
		});
		GLFW.SetCursorPosCallback(Window, (_, x, y) =>
		{
			OGLRemoteKeyboard state = (OGLRemoteKeyboard) RemoteKeyboard.Global;
			state.Cursor.x = (float) x;
			state.Cursor.y = (float) (GraphicsDevice.Global.Size.y - y);
		});
		GLFW.SetScrollCallback(Window, (_, x, y) =>
		{
			OGLRemoteKeyboard state = (OGLRemoteKeyboard) RemoteKeyboard.Global;
			state.ScrollUABS = (float) y;
		});
		GLFW.SetWindowSizeCallback(Window, (_, width, height) =>
		{
			Application._CallResize();
			Pw = width;
			Ph = height;
		});
	}

}
