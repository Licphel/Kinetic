using System.Drawing;
using System.Drawing.Imaging;
using Kinetic.App;
using Kinetic.IO;
using OpenTK.Graphics.OpenGL;
using StbImageSharp;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using Visual_Texture = Kinetic.Visual.Texture;

//using System.Drawing;
//using System.Drawing.Imaging;

namespace Kinetic.OpenGL;

public class OGLTexture : Visual_Texture
{

	public string FileSrc;
	public ImageResult _N_RES;

	public int Id;
	public bool IsFB;

	static OGLTexture()
	{
		GL.Enable(EnableCap.Texture2D);
	}

	public OGLTexture(int id, int w, int h)
	{
		Id = id;
		Width = w;
		Height = h;
	}

	public OGLTexture(FileHandle handler)
	{
		FileSrc = handler.Path;

		Id = GL.GenTexture();

		GL.ActiveTexture(TextureUnit.Texture0);
		GL.BindTexture(TextureTarget.Texture2D, Id);

		ImageResult result = _N_RES = ImageResult.FromMemory(File.ReadAllBytes(handler.Path), ColorComponents.RedGreenBlueAlpha);

		Width = result.Width;
		Height = result.Height;

		GL.TexImage2D(
			TextureTarget.Texture2D,
			0,
			PixelInternalFormat.Rgba,
			Width,
			Height,
			0,
			PixelFormat.Rgba,
			PixelType.UnsignedByte,
			result.Data
		);
		
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
			(int) OGLDeviceSettings.FilterMag);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
			(int) OGLDeviceSettings.FilterMin);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
			(int) OGLDeviceSettings.Wrap);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
			(int) OGLDeviceSettings.Wrap);

		if(OGLDeviceSettings.MipmapLevel > 0)
		{
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBaseLevel, 0);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMaxLevel,
				OGLDeviceSettings.MipmapLevel);
			GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
		}

		NativeManager.I0.Remind(() => GL.DeleteTexture(Id));
	}

	public OGLTexture()
	{
		FileSrc = "";
		Id = GL.GenTexture();

		GL.ActiveTexture(TextureUnit.Texture0);
		GL.BindTexture(TextureTarget.Texture2D, Id);

		NativeManager.I0.Remind(() => GL.DeleteTexture(Id));
	}
	
	public void Upload(Bitmap bmap)
	{
		GL.ActiveTexture(TextureUnit.Texture0);
		GL.BindTexture(TextureTarget.Texture2D, Id);

		Width = bmap.Width;
		Height = bmap.Height;

		var nb = bmap.Clone() as Bitmap;
		
		BitmapData data = nb.LockBits(new(0, 0, Width, Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

		GL.TexImage2D(
			TextureTarget.Texture2D,
			0,
			PixelInternalFormat.Rgba,
			Width,
			Height,
			0,
			OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
			PixelType.UnsignedByte,
			data.Scan0
		);
		
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
			(int)OGLDeviceSettings.FilterMag);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
			(int)OGLDeviceSettings.FilterMin);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
			(int)OGLDeviceSettings.Wrap);
		GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
			(int)OGLDeviceSettings.Wrap);
		
		//IDK Why I cannot dispose the bitmaps.
		//It will throw an exception when I try.
		//bmap.Dispose();
		nb.Dispose();
	}

}
