using Kinetic.App;
using OpenTK.Graphics.OpenGL;

namespace Kinetic.OpenGL;

public class Bufferobject<T> where T : unmanaged
{

	readonly BufferTarget type;

	public int Id;

	public unsafe Bufferobject(T* data, int len, BufferTarget target, BufferUsageHint hint)
	{
		type = target;
		Id = GL.GenBuffer();

		Bind();

		GL.BufferData(type, len * sizeof(T), (IntPtr) data, hint);

		NativeManager.I0.Remind(() => GL.DeleteBuffer(Id));
	}

	public void Bind()
	{
		GL.BindBuffer(type, Id);
	}

	public void Unbind()
	{
		GL.BindBuffer(type, 0);
	}

	public unsafe void UpdateBuffer(nint offset, T* data, int len)
	{
		GL.BufferSubData(type, offset, len * sizeof(T), (IntPtr) data);
	}

}

public class VertArrayobject<T, I>
where T : unmanaged
where I : unmanaged
{

	public int Id;

	public VertArrayobject(Bufferobject<T> vbo, Bufferobject<I> ebo)
	{
		Id = GL.GenVertexArray();

		Bind();
		vbo.Bind();
		ebo?.Bind();

		NativeManager.I0.Remind(() => GL.DeleteVertexArray(Id));
	}

	public void Bind()
	{
		GL.BindVertexArray(Id);
	}

	public void Unbind()
	{
		GL.BindVertexArray(0);
	}

}
