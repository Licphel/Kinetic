using Kinetic.Math;
using Kinetic.Visual;
using OpenTK.Graphics.OpenGL;

namespace Kinetic.OpenGL;

public class OGLTextureBuffer : TextureBuffer
{

	readonly Vector4 viewport;
	public int Id;
	Vector4 tmpVp;

	public OGLTextureBuffer(int w, int h)
	{
		Id = GL.GenFramebuffer();
		int tid = GL.GenTexture();

		GL.BindFramebuffer(FramebufferTarget.Framebuffer, Id);
		GL.BindTexture(TextureTarget.Texture2d, tid);

		GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMagFilter,
			(int) OGLDeviceSettings.FilterMag);
		GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureMinFilter,
			(int) OGLDeviceSettings.FilterMin);
		GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapS,
			(int) TextureWrapMode.Repeat);
		GL.TexParameteri(TextureTarget.Texture2d, TextureParameterName.TextureWrapT,
			(int) TextureWrapMode.Repeat);

		GL.TexImage2D(
			TextureTarget.Texture2d,
			0,
			InternalFormat.Rgba,
			w,
			h,
			0,
			PixelFormat.Rgba,
			PixelType.UnsignedByte,
			(byte[]) null
		);
		GL.FramebufferTexture2D
		(
			FramebufferTarget.Framebuffer,
			FramebufferAttachment.ColorAttachment0,
			TextureTarget.Texture2d,
			tid,
			0
		);

		GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);

		Src = new OGLTexture(tid, w, h);
		((OGLTexture) Src).IsFB = true;

		viewport = new Vector4(0, 0, w, h);
	}

	public Texture Src { get; }

	public void Begin(SpriteBatch batch)
	{
		tmpVp = batch.ViewportArray;
		batch.Flush();
		batch.Viewport(viewport);
		GL.BindFramebuffer(FramebufferTarget.Framebuffer, Id);
		GL.ClearColor(OGLUtil.ToColor(OGLDevice.Settings.ClearColor));
		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
	}

	public void End(SpriteBatch batch)
	{
		batch.Flush();
		batch.Viewport(tmpVp);
		GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
	}

	public void Free()
	{
		GL.DeleteFramebuffer(Id);
		GL.DeleteTexture(((OGLTexture) Src).Id);
	}

}
