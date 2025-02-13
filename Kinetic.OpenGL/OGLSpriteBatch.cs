using System.Runtime.CompilerServices;
using Kinetic.App;
using Kinetic.Math;
using Kinetic.Visual;
using OpenTK.Graphics.OpenGL;
using Draw_Texture = Kinetic.Visual.Texture;

namespace Kinetic.OpenGL;

public unsafe class OGLSpriteBatch : SpriteBatch
{

	static readonly PerspectiveCamera NullCam = new PerspectiveCamera();
	public Bufferobject<uint> Ebo;

	public uint* Indices;
	public int Indlen;
	float InvTexHeight;
	float InvTexWidth;

	public int MinVertexBufSize = 256;
	public int NumIndices;
	public int NumVertices;

	public GLShaderProgram ProgramDefault;

	int TextureID;
	ShaderUniform UniProjection;

	ShaderUniform UniTexture;

	public VertArrayobject<float, uint> Vao;
	public Bufferobject<float> Vbo;
	public float* Vertices;
	public float* Vertice0;
	public int Verlen;

	public OGLSpriteBatch(int size)
	{
		NormalizeColor();
		//1 sprite - 4 vert - 6 ind
		Vertice0 = Vertices = NativeMem.MemReallocate(Vertices, Verlen = size, 0);
		Indices = NativeMem.MemReallocate(Indices, Indlen = size / 2 * 3, 0);

		for(uint i = 0, k = 0; i < size / 4; i += 6, k += 4)
		{
			Indices[i + 0] = 0 + k;
			Indices[i + 1] = 1 + k;
			Indices[i + 2] = 3 + k;
			Indices[i + 3] = 1 + k;
			Indices[i + 4] = 2 + k;
			Indices[i + 5] = 3 + k;
		}

		GL.Enable(EnableCap.Blend);
		GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

		Ebo = new Bufferobject<uint>(Indices, Indlen, BufferTarget.ElementArrayBuffer, BufferUsageHint.DynamicDraw);
		Vbo = new Bufferobject<float>(Vertices, Verlen, BufferTarget.ArrayBuffer, BufferUsageHint.DynamicDraw);
		Vao = new VertArrayobject<float, uint>(Vbo, Ebo);

		ProgramDefault = GetDefaultShader();

		Load(ProgramDefault);

		UniTexture = ProgramDefault.GetUniform("u_texture");

		//Get as default
		ViewportArray = new Vector4(0, 0, GraphicsDevice.Global.Size.x, GraphicsDevice.Global.Size.y);
	}

	public void Load(GLShaderProgram program)
	{
		Flush();

		Vao.Bind();

		Program = program;
		program.Bind();
		program.Setup();

		UniProjection = program.GetUniform("u_proj");
		UniProjection.SetMat4(Projection);
	}

	public void Unload()
	{
		Load(ProgramDefault);
	}

	public override void UseShader(ShaderProgram program)
	{
		Load((GLShaderProgram) program);
	}

	public override void UseDefaultShader()
	{
		Load(ProgramDefault);
	}

	public override void Draw(Draw_Texture texture, float x, float y, float width, float height, float srcX, float srcY,
	                          float srcWidth, float srcHeight)
	{
		texture = texture ?? TexMissing;

		OGLTexture glt = (OGLTexture) texture;

		CheckTransformAndCap();

		int id = glt.Id;

		if(id != TextureID)
		{
			InvTexWidth = 1f / texture.Width;
			InvTexHeight = 1f / texture.Height;
			Flush();
		}

		TextureID = id;

		float x1;
		float y1;
		float x2;
		float y2;
		float x3;
		float y3;
		float x4;
		float y4;

		if(!Matrices.IsEmpty)
		{
			x1 = x + Transform.m02;
			y1 = y + Transform.m12;
			x2 = x + Transform.m01 * height + Transform.m02;
			y2 = y + Transform.m11 * height + Transform.m12;
			x3 = x + Transform.m00 * width + Transform.m01 * height + Transform.m02;
			y3 = y + Transform.m10 * width + Transform.m11 * height + Transform.m12;
			x4 = x + Transform.m00 * width + Transform.m02;
			y4 = y + Transform.m10 * width + Transform.m12;
		}
		else
		{
			x1 = x;
			y1 = y;
			x2 = x;
			y2 = y + height;
			x3 = x + width;
			y3 = y + height;
			x4 = x + width;
			y4 = y;
		}

		float u = srcX * InvTexWidth;
		float v = srcY * InvTexHeight;
		float u2 = (srcX + srcWidth) * InvTexWidth;
		float v2 = (srcY + srcHeight) * InvTexHeight;

		if(FlipX)
		{
			(u, u2) = (u2, u);
		}

		if(FlipY)
		{
			(v, v2) = (v2, v);
		}

		if(glt.IsFB)
		{
			(v, v2) = (v2, v);
		}

		{
			//RT
			*Vertices++ = x3;
			*Vertices++ = y3;
			*Vertices++ = LinearCol4[2].x;
			*Vertices++ = LinearCol4[2].y;
			*Vertices++ = LinearCol4[2].z;
			*Vertices++ = LinearCol4[2].w;
			*Vertices++ = u2;
			*Vertices++ = v;

			VertAppenders[2]?.Invoke(this);

			//RD
			*Vertices++ = x4;
			*Vertices++ = y4;
			*Vertices++ = LinearCol4[3].x;
			*Vertices++ = LinearCol4[3].y;
			*Vertices++ = LinearCol4[3].z;
			*Vertices++ = LinearCol4[3].w;
			*Vertices++ = u2;
			*Vertices++ = v2;

			VertAppenders[3]?.Invoke(this);

			//LD
			*Vertices++ = x1;
			*Vertices++ = y1;
			*Vertices++ = LinearCol4[0].x;
			*Vertices++ = LinearCol4[0].y;
			*Vertices++ = LinearCol4[0].z;
			*Vertices++ = LinearCol4[0].w;
			*Vertices++ = u;
			*Vertices++ = v2;

			VertAppenders[0]?.Invoke(this);

			//LT
			*Vertices++ = x2;
			*Vertices++ = y2;
			*Vertices++ = LinearCol4[1].x;
			*Vertices++ = LinearCol4[1].y;
			*Vertices++ = LinearCol4[1].z;
			*Vertices++ = LinearCol4[1].w;
			*Vertices++ = u;
			*Vertices++ = v;

			VertAppenders[1]?.Invoke(this);
		}

		NewVertex(32);
		NewIndex(6);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void CheckTransformAndCap()
	{
		if(Verlen - (Vertices - Vertice0) < MinVertexBufSize)
		{
			Flush();
		}

		if(Matrices.Changed)
		{
			Transform.Set(Matrices.Top);
			Matrices.Changed = false;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void Write(float v)
	{
		*Vertices++ = v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void Write(float v1, float v2)
	{
		*Vertices++ = v1;
		*Vertices++ = v2;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void Write(float v1, float v2, float v3)
	{
		*Vertices++ = v1;
		*Vertices++ = v2;
		*Vertices++ = v3;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void Write(Vector2 vec)
	{
		*Vertices++ = vec.x;
		*Vertices++ = vec.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void Write(Vector3 vec)
	{
		*Vertices++ = vec.x;
		*Vertices++ = vec.y;
		*Vertices++ = vec.z;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void Write(Vector4 vec)
	{
		*Vertices++ = vec.x;
		*Vertices++ = vec.y;
		*Vertices++ = vec.z;
		*Vertices++ = vec.w;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void Write(params float[] arr)
	{
		for(int i = 0; i < arr.Length; i++)
		{
			*Vertices++ = arr[i];
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void WriteTransformed(Vector2 vec)
	{
		Vector2 vecTrf = Transform.ApplyTo(ref vec);

		*Vertices++ = vecTrf.x;
		*Vertices++ = vecTrf.y;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void NewVertex(int v)
	{
		NumVertices += v;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override void NewIndex(int v)
	{
		NumIndices += v;
	}

	public override void Flush()
	{
		if(NumVertices <= 0)
		{
			return;
		}

		Program.Bind();

		UniTexture.SetTexUnit(TextureID, 0);
		UniProjection.SetMat4(Projection);
		UniformAppender?.Invoke(this);

		Vao.Bind();
		Vbo.Bind();
		Ebo.Bind();
		Vbo.UpdateBuffer(0, Vertice0, NumVertices);

		GL.DrawElements(BeginMode.Triangles, NumIndices, DrawElementsType.UnsignedInt, 0);

		Program.Unbind();

		_DrawCalls++;

		NumVertices = 0;
		NumIndices = 0;
		Vertices = Vertice0;
	}

	public override void Viewport(Vector4 viewport)
	{
		Viewport(viewport.x, viewport.y, viewport.z, viewport.w);
	}

	public override void Viewport(float x, float y, float w, float h)
	{
		GL.Viewport((int) x, (int) y, (int) w, (int) h);
		ViewportArray = new Vector4(x, y, w, h);
	}

	public override void Scissor(PerspectiveCamera camera, Vector4 viewport, float x, float y, float w, float h)
	{
		Flush();
		float x0 = camera.ToScrX(x, viewport);
		float y0 = camera.ToScrY(y, viewport);
		float x1 = camera.ToScrX(x + w, viewport);
		float y1 = camera.ToScrY(y + h, viewport);
		GL.Scissor((int) x0 - 1, (int) y0 - 1, (int) (x1 - x0 + 1), (int) (y1 - y0 + 1));
		GL.Enable(EnableCap.ScissorTest);
	}

	public override void ScissorEnd()
	{
		Flush();
		GL.Disable(EnableCap.ScissorTest);
	}

	public override void Clear()
	{
		GL.ClearColor(OGLUtil.ToColor(OGLDevice.Settings.ClearColor));
		GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
	}

	public override void UseCamera(PerspectiveCamera camera)
	{
		Flush();
		Projection.ToAffine(camera.CombinedAffine);
		UniProjection.SetMat4(Projection);
		CameraNow = camera;
	}

	public override void EndCamera(PerspectiveCamera camera)
	{
		NullCam.Viewport.Set(0, 0, GraphicsDevice.Global.Size.x, GraphicsDevice.Global.Size.y);
		NullCam.ToCenter();
		NullCam.Push();
		UseCamera(NullCam);
	}

	//-----

	static GLShaderProgram GetDefaultShader()
	{
		const string vert = "#version 150 core\n" +
		                    "\n" +
		                    "in vec2 i_position;\n" +
		                    "in vec4 i_color;\n" +
		                    "in vec2 i_texCoord;\n" +
		                    "\n" +
		                    "out vec4 o_color;\n" +
		                    "out vec2 o_texCoord;\n" +
		                    "\n" +
		                    "uniform mat4 u_proj;\n" +
		                    "\n" +
		                    "void main() {\n" +
		                    "    o_color = i_color;\n" +
		                    "    o_texCoord = i_texCoord;\n" +
		                    "\n" +
		                    "    gl_Position = u_proj * vec4(i_position, 0.0, 1.0);\n" +
		                    "}\n";
		const string frag = "#version 150 core\n" +
		                    "\n" +
		                    "in vec4 o_color;\n" +
		                    "in vec2 o_texCoord;\n" +
		                    "\n" +
		                    "out vec4 fragColor;\n" +
		                    "\n" +
		                    "uniform sampler2D u_texture;\n" +
		                    "\n" +
		                    "void main() {\n" +
		                    "    fragColor = o_color * texture(u_texture, o_texCoord);\n" +
		                    "}";
		return ShaderBuilds.Build(vert, frag, program =>
		{
			ShaderAttribute posAttrib = program.GetAttribute("i_position");
			posAttrib.Enable();
			ShaderAttribute colAttrib = program.GetAttribute("i_color");
			colAttrib.Enable();
			ShaderAttribute texAttrib = program.GetAttribute("i_texCoord");
			texAttrib.Enable();

			posAttrib.Ptr(VertexAttribPointerType.Float, 2, 32, 0);
			colAttrib.Ptr(VertexAttribPointerType.Float, 4, 32, 8);
			texAttrib.Ptr(VertexAttribPointerType.Float, 2, 32, 24);
		});
	}

}
