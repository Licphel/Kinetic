using Kinetic.Math;

namespace Kinetic.Visual;

public abstract class SpriteBatch
{

	public static SpriteBatch Global;

	public static int DefaultSize = 1024 * 8;

	public int _DrawCalls = 0;
	public PerspectiveCamera CameraNow;

	public bool FlipX;
	public bool FlipY;

	public Font Font;
	public FontCarver FontCarver = new DefaultFontCarver();

	public Vector4[] LinearCol4 = new Vector4[4] { Vector4.One, Vector4.One, Vector4.One, Vector4.One };
	public Stack<Vector4[]> ColorCache = new Stack<Vector4[]>();
	public MatrixStack Matrices = new MatrixStack();

	public ShaderProgram Program;
	public Matrix Projection = new Matrix();

	public Texture Texfil;
	public Texture TexMissing;

	public Affine Transform = new Affine();
	public UniformAppender UniformAppender;

	public VertexAppender[] VertAppenders = new VertexAppender[4];
	public Vector4 ViewportArray;

	public abstract void Draw(Texture texture, float x, float y, float width, float height, float srcX, float srcY,
		float srcWidth, float srcHeight);

	public void Draw(Texture tex, float x, float y, float sx, float sy, float sw, float sh)
	{
		Draw(tex, x, y, sw, sh, sx, sy, sw, sh);
	}

	public void Draw(Texture tex, float x, float y, float w, float h)
	{
		Draw(tex, x, y, w, h, 0, 0, tex.Width, tex.Height);
	}

	public void Draw(Texture tex, float x, float y)
	{
		Draw(tex, x, y, tex.Width, tex.Height, 0, 0, tex.Width, tex.Height);
	}

	public void Draw(Texture tex, Box rect, float sx, float sy, float sw, float sh)
	{
		Draw(tex, rect.x, rect.y, rect.w, rect.h, sx, sy, sw, sh);
	}

	public void Draw(Texture tex, Box rect)
	{
		Draw(tex, rect.x, rect.y, rect.w, rect.h);
	}

	public void Draw(Icon icon, float x, float y, float w, float h)
	{
		icon?.Draw(this, x, y, w, h);
	}

	public void Draw(Icon icon, Box rect)
	{
		Draw(icon, rect.x, rect.y, rect.w, rect.h);
	}

	//This method use the textured mode to present a rectangle.
	//Most of the time this is a much better choice.
	public void Fill(float x, float y, float width, float height)
	{
		Draw(Texfil, x, y, width, height);
	}

	public void Fill(Box rect)
	{
		Fill(rect.x, rect.y, rect.w, rect.h);
	}

	public abstract void CheckTransformAndCap();

	public abstract void Write(Vector2 vec);
	public abstract void Write(Vector3 vec);
	public abstract void Write(Vector4 vec);
	public abstract void Write(params float[] arr);
	public abstract void Write(float f1);
	public abstract void Write(float f1, float f2);
	public abstract void Write(float f1, float f2, float f3);
	public abstract void WriteTransformed(Vector2 vec);
	public abstract void NewVertex(int v);
	public abstract void NewIndex(int v);

	public abstract void Flush();

	public void Color4(float r, float g, float b, float a = 1)
	{
		LinearCol4[0] = LinearCol4[1] = LinearCol4[2] = LinearCol4[3] = new Vector4(r, g, b, a);
	}

	public void Color4(Vector3 vector3, float a = 1)
	{
		LinearCol4[0] = LinearCol4[1] = LinearCol4[2] = LinearCol4[3] = new Vector4(vector3.x, vector3.y, vector3.z, a);
	}

	public void Color4(Vector4 color)
	{
		LinearCol4[0] = LinearCol4[1] = LinearCol4[2] = LinearCol4[3] = color;
	}

	public void Merge4(float r, float g, float b, float a = 1)
	{
		LinearCol4[0] *= new Vector4(r, g, b, a);
		LinearCol4[1] *= new Vector4(r, g, b, a);
		LinearCol4[2] *= new Vector4(r, g, b, a);
		LinearCol4[3] *= new Vector4(r, g, b, a);
	}

	public void Merge4(Vector3 vector3, float a = 1)
	{
		LinearCol4[0] *= new Vector4(vector3.x, vector3.y, vector3.z, a);
		LinearCol4[1] *= new Vector4(vector3.x, vector3.y, vector3.z, a);
		LinearCol4[2] *= new Vector4(vector3.x, vector3.y, vector3.z, a);
		LinearCol4[3] *= new Vector4(vector3.x, vector3.y, vector3.z, a);
	}

	public void Merge4(Vector4 color)
	{
		LinearCol4[0] *= color;
		LinearCol4[1] *= color;
		LinearCol4[2] *= color;
		LinearCol4[3] *= color;
	}

	public void NormalizeColor()
	{
		LinearCol4[0] = LinearCol4[1] = LinearCol4[2] = LinearCol4[3] = new Vector4(1, 1, 1, 1);
	}

	public void PushColor()
	{
		ColorCache.Push(new Vector4[] {LinearCol4[0], LinearCol4[1], LinearCol4[2], LinearCol4[3]});
	}

	public void PopColor()
	{
		LinearCol4 = ColorCache.Pop();
	}

	public void Scissor(float x, float y, float w, float h)
	{
		Scissor(CameraNow, ViewportArray, x, y, w, h);
	}

	public abstract void Viewport(Vector4 viewport);
	public abstract void Viewport(float x, float y, float w, float h);
	public abstract void Scissor(PerspectiveCamera camera, Vector4 viewport, float x, float y, float w, float h);
	public abstract void ScissorEnd();
	public abstract void Clear();
	public abstract void UseCamera(PerspectiveCamera camera);
	public abstract void EndCamera(PerspectiveCamera camera);
	public abstract void UseShader(ShaderProgram program);
	public abstract void UseDefaultShader();

	public void UseVertAppenders(VertexAppender[] appds)
	{
		Array.Copy(appds, VertAppenders, 4);
	}

	public void EndVertAppenders()
	{
		VertAppenders[0] = VertAppenders[1] = VertAppenders[2] = VertAppenders[3] = null;
	}

	//Font Rendering:
	//For more functions see #Lore.

	public void Draw(string text, float x, float y, float maxWidth = int.MaxValue)
	{
		FontCarver.Draw(this, Font, text, x, y, maxWidth);
	}

	public void Draw(string text, float x, float y, Align align, float maxWidth = int.MaxValue)
	{
		GlyphBounds bounds = Font.GetBounds(text);

		switch(align)
		{
			case Align.LEFT:
				Draw(text, x, y, maxWidth);
				break;
			case Align.RIGHT:
				Draw(text, x - bounds.Width, y, maxWidth);
				break;
			case Align.CENTER:
				Draw(text, x - bounds.Width / 2.0F, y, maxWidth);
				break;
		}
	}

	public void Draw(Lore text, float x, float y, float maxWidth = int.MaxValue)
	{
		FontCarver.Draw(this, Font, text, x, y, maxWidth);
	}

	public void Draw(Lore text, float x, float y, Align align, float maxWidth = int.MaxValue)
	{
		if(text.Content.Count == 0) return;

		GlyphBounds bounds = Font.GetBounds(text);

		switch(align)
		{
			case Align.LEFT:
				Draw(text, x, y, maxWidth);
				break;
			case Align.RIGHT:
				Draw(text, x - bounds.Width, y, maxWidth);
				break;
			case Align.CENTER:
				Draw(text, x - bounds.Width / 2.0F, y, maxWidth);
				break;
		}
	}

}

public delegate void VertexAppender(SpriteBatch batch);
public delegate void UniformAppender(SpriteBatch batch);
