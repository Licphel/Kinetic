using System.Drawing;
using System.Drawing.Imaging;
using Kinetic.App;
using Kinetic.IO;
using OpenTK.Graphics.OpenGL;
using StbImageSharp;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using Visual_Texture = Kinetic.Visual.Texture;

namespace Kinetic.OpenGL;

public class OGLTexture : Visual_Texture
{

	public string FileSrc;
	public ImageResult _N_RES;

	public int Id;
	public bool IsFB;

	static OGLTexture()
	{
		GL.Enable(EnableCap.Texture2d);
	}

	public OGLTexture(int id, int w, int h)
	{
		Id = id;
		uw = w;
		vh = h;
	}

	public OGLTexture(FileHandle handler)
	{
		FileSrc = handler.Path;

		Id = GL.GenTexture();

		GL.ActiveTexture(TextureUnit.Texture0);
		GL.BindTexture(TextureTarget.Texture2d, Id);

		ImageResult result = _N_RES = ImageResult.FromMemory(File.ReadAllBytes(handler.Path), ColorComponents.RedGreenBlueAlpha);

		uw = result.Width;
		vh = result.Height;

		GL.TexImage2D(
			TextureTarget.Texture2d,
			0,
			InternalFormat.Rgba,
			Width,
			Height,
			0,
			PixelFormat.Rgba,
			PixelType.UnsignedByte,
			result.Data
		);
		
		GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter,
			(int) OGLDeviceSettings.FilterMag);
		GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter,
			(int) OGLDeviceSettings.FilterMin);
		GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS,
			(int) OGLDeviceSettings.Wrap);
		GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT,
			(int) OGLDeviceSettings.Wrap);

		if(OGLDeviceSettings.MipmapLevel > 0)
		{
			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureBaseLevel, 0);
			GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMaxLevel,
				OGLDeviceSettings.MipmapLevel);
			GL.GenerateMipmap(TextureTarget.Texture2d);
		}

		NativeManager.I0.Remind(() => GL.DeleteTexture(Id));
	}

	public OGLTexture()
	{
		FileSrc = "";
		Id = GL.GenTexture();

		GL.ActiveTexture(TextureUnit.Texture0);
		GL.BindTexture(TextureTarget.Texture2d, Id);

		NativeManager.I0.Remind(() => GL.DeleteTexture(Id));
	}
	
	public void Upload(Bitmap bmap)
	{
		GL.ActiveTexture(TextureUnit.Texture0);
		GL.BindTexture(TextureTarget.Texture2d, Id);

		uw = bmap.Width;
		vh = bmap.Height;

		var nb = bmap.Clone() as Bitmap;
		
		BitmapData data = nb.LockBits(new(0, 0, Width, Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

		GL.TexImage2D(
			TextureTarget.Texture2d,
			0,
			InternalFormat.Rgba,
			Width,
			Height,
			0,
			OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
			PixelType.UnsignedByte,
			data.Scan0
		);
		
		GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter,
			(int)OGLDeviceSettings.FilterMag);
		GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter,
			(int)OGLDeviceSettings.FilterMin);
		GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS,
			(int)OGLDeviceSettings.Wrap);
		GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT,
			(int)OGLDeviceSettings.Wrap);
		
		//Idk why I cannot dispose the bitmap.
		//bmap.Dispose();
		nb.Dispose();
	}

}
