using Kinetic.IO;
using Kinetic.Math;
using OpenTK.Graphics.OpenGL;

namespace Kinetic.OpenGL;

public class OGLDeviceSettings
{

	public static int MipmapLevel = 0;
	public static TextureMagFilter FilterMag = TextureMagFilter.Nearest;
	public static TextureMinFilter FilterMin = TextureMinFilter.Nearest;
	public static TextureWrapMode Wrap = TextureWrapMode.Repeat;
	public bool AutoIconify = true;

	public Vector4 ClearColor = new Vector4(0, 0, 0, 1);
	public FileHandle Cursor;
	public bool Decorated = true;
	public bool Floating = false;
	public Vector2 Hotspot = new Vector2(0, 0);
	public FileHandle[] Icons;
	public bool Maximized = false;
	public bool Resizable = true;

	public Vector2 Size = new Vector2(128, 128);

	public string Title = "Kinetic OpenGL";

}
